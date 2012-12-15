//-----------------------------------------------------------------------
// <copyright file="Personnage.cs" company="Marco Lavoie">
// Marco Lavoie, 2010. Tous droits réservés
// 
// L'utilisation de ce matériel pédagogique (présentations, code source 
// et autres) avec ou sans modifications, est permise en autant que les 
// conditions suivantes soient respectées:
//
// 1. La diffusion du matériel doit se limiter à un intranet dont l'accès
//    est imité aux étudiants inscrits à un cours exploitant le dit 
//    matériel. IL EST STRICTEMENT INTERDIT DE DIFFUSER CE MATÉRIEL 
//    LIBREMENT SUR INTERNET.
// 2. La redistribution des présentations contenues dans le matériel 
//    pédagogique est autorisée uniquement en format Acrobat PDF et sous
//    restrictions stipulées à la condition #1. Le code source contenu 
//    dans le matériel pédagogique peut cependant être redistribué sous 
//    sa forme  originale, en autant que la condition #1 soit également 
//    respectée.
// 3. Le matériel diffusé doit contenir intégralement la mention de 
//    droits d'auteurs ci-dessus, la notice présente ainsi que la
//    décharge ci-dessous.
// 
// CE MATÉRIEL PÉDAGOGIQUE EST DISTRIBUÉ "TEL QUEL" PAR L'AUTEUR, SANS 
// AUCUNE GARANTIE EXPLICITE OU IMPLICITE. L'AUTEUR NE PEUT EN AUCUNE 
// CIRCONSTANCE ÊTRE TENU RESPONSABLE DE DOMMAGES DIRECTS, INDIRECTS, 
// CIRCONSTENTIELS OU EXEMPLAIRES. TOUTE VIOLATION DE DROITS D'AUTEUR 
// OCCASIONNÉ PAR L'UTILISATION DE CE MATÉRIEL PÉDAGOGIQUE EST PRIS EN 
// CHARGE PAR L'UTILISATEUR DU DIT MATÉRIEL.
// 
// En utilisant ce matériel pédagogique, vous acceptez implicitement les
// conditions et la décharge exprimés ci-dessus.
// </copyright>
//-----------------------------------------------------------------------

namespace IFM20884
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Storage;

    /// <summary>
    /// Définition de fonction déléguée permettant de calculer la résistance aux déplacements
    /// dans le monde à la position donnée.
    /// </summary>
    /// <param name="position">Position du pixel en coordonnées du monde.</param>
    /// <returns>Facteur de résistance entre 0.0f (aucune résistance) et 1.0f (résistance maximale).</returns>
    public delegate float ResistanceAuMouvement(Vector2 position);

    /// <summary>
    /// Définition de fonction déléguée permettant de valider un déplacement d'une position
    /// à une autre dans le monde. La fonction retourne le point le plus près de 
    /// (posSource.X+deltaX, posSource.Y+DeltaY) jusqu'où le personnage peut se rendre horizontalement 
    /// et verticalement sans rencontrer de résistance plus élévée que la limite donnée.
    /// </summary>
    /// <param name="posSource">Position du pixel de départ du déplacement, en coordonnées du monde.</param>
    /// <param name="deltaX">Déplacement total horizontal, en coordonnées du monde.</param>
    /// <param name="deltaY">Déplacement total vertical, en coordonnées du monde.</param>
    /// <param name="resistanceMax">Résistance maximale tolérée lors du déplacement.</param>
    public delegate void ValiderDeplacement(Vector2 posSource, ref int deltaX, ref int deltaY, float resistanceMax);

    /// <summary>
    /// Classe implantant le sprite représentant un personnage pouvant être stationnairte, marcher 
    /// et courir dans les huit directions.
    /// </summary>
    public abstract class Personnage : SpriteAnimation
    {
        /// <summary>
        /// Fonction déléguée permettant d'obtenir la résistance aux déplacements du sprite
        /// dans le monde de tuiles. Si aucune fonction déléguée n'est fournie, aucune
        /// résistance n'est appliquée aux déplacements.
        /// </summary>
        private ResistanceAuMouvement getResistanceAuMouvement;

        /// <summary>
        /// Fonction déléguée permettant de valider les déplacements du sprite
        /// dans le monde de tuiles. Si aucune fonction déléguée n'est fournie, aucune
        /// résistance n'est appliquée aux déplacements.
        /// </summary>
        private ValiderDeplacement getValiderDeplacement;

        /// <summary>
        /// Vitesse de marche du personnage, avec valeur par défaut.
        /// </summary>
        private float vitesseMaximum = 0.35f;

        /// <summary>
        /// Attribut indiquant la direction de déplacement courante.
        /// </summary>
        private Directions direction;

        /// <summary>
        /// Attribut indiquant la direction de déplacement courante.
        /// </summary>
        private Etats etat;

        /// <summary>
        /// Instance de bruitage des moteurs en cours de sonorisation durant le jeu.
        /// </summary>
        private SoundEffectInstance bruitActif;

        /// <summary>
        /// Attribut représentant la camera.
        /// </summary>
        private Camera camera = null;

        /// <summary>
        /// Attribut indiquant la vie du joueur
        /// </summary>
        private int vieDeJoueur;

        /// <summary>
        /// Attribut indiquant la vie du joueur
        /// </summary>
        private bool clef;

        /// <summary>
        /// Attribut qui sert à controller l'angle de rotation l'hors d'un tombe.
        /// </summary>
        private float angleRotation = 0.0f;
        
        /// <summary>
        /// Atribut representant l'echelle pendant une tombe.
        /// </summary>
        private float echelle = 1.0f;       

        /// <summary>
        /// Attribut s'assure qu'un personnage sort d'état du tombe.
        /// </summary>
        private int contTombe = 0;

        /// <summary>
        /// La vitesse horizontal du personnage.
        /// </summary>
        private float vitesseHorizontal;

        /// <summary>
        /// La vitesse vertical du personnage.
        /// </summary>
        private float vitesseVerticale;
        
        /// <summary>
        /// Constructeur paramétré recevant la position du sprite.
        /// </summary>
        /// <param name="x">Coordonnée initiale x (horizontale) du sprite.</param>
        /// <param name="y">Coordonnée initiale y (verticale) du sprite.</param>
        public Personnage(float x, float y)
            : base(x, y)
        {
            // Par défaut, le sprite est celui faisant face aux Joueur du partie.
            this.direction = Directions.Sud;
            this.etat = Etats.Stationnaire;
        }

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite. On invoque l'autre constructeur.
        /// </summary>
        /// <param name="position">Coordonnées initiales horizontale et verticale du sprite.</param>
        public Personnage(Vector2 position)
            : this(position.X, position.Y)
        {
        }

        /// <summary>
        /// Enumération des directions potentielles de déplacement du personnage.
        /// </summary>
        public enum Directions 
        {
            /// <summary>
            /// Déplacement vers le haut de l'écran.
            /// </summary>
            Nord,

            /// <summary>
            /// Déplacement vers le coin supérieur gauche de l'écran.
            /// </summary>
            NordEst,

            /// <summary>
            /// Déplacement vers la gauche l'écran.
            /// </summary>
            Est,

            /// <summary>
            /// Déplacement vers le coin inférieur gauche de l'écran.
            /// </summary>
            SudEst,

            /// <summary>
            /// Déplacement vers le bas de l'écran.
            /// </summary>
            Sud,

            /// <summary>
            /// Déplacement vers le coin inférieur droit de l'écran.
            /// </summary>
            SudOuest,

            /// <summary>
            /// Déplacement vers la droite de l'écran.
            /// </summary>
            Ouest,

            /// <summary>
            /// Déplacement vers le coin supérieur droit de l'écran.
            /// </summary>
            NordOuest
        }

        /// <summary>
        /// Enumération des états disponibles du personnage.
        /// </summary>
        public enum Etats 
        {
            /// <summary>
            /// Le personnage ne se déplace pas.
            /// </summary>
            Stationnaire,

            /// <summary>
            /// Le personnage se déplace lentement.
            /// </summary>
            Marche,

            /// <summary>
            /// Le personnage se déplace au pas de course.
            /// </summary>
            Course,

            /// <summary>
            /// Le personnage tombe dans un trou.
            /// </summary>
            Tombe,

            /// <summary>
            /// Le personnage mort. Seulement implémenté pour le joueur.
            /// </summary>
            Mort 
        }

        /// <summary>
        /// Public vie de joueur, pour la vie du joueur
        /// </summary>
        public int VieDeJoueur
        {
            get { return this.vieDeJoueur; }
            set { this.vieDeJoueur = value; }
        }

        /// <summary>
        /// Accesseur pour attribut vitesseMaximum.
        /// </summary>
        public float AngleRotation
        {
            get { return this.angleRotation; }
            set { this.angleRotation = value; }
        }

        /// <summary>
        /// Accesseur pour attribut vitesseMaximum.
        /// </summary>
        public float VitesseMaximum
        {
            get { return this.vitesseMaximum; }
            set { this.vitesseMaximum = value; }
        }

        /// <summary>
        /// Accesseur pour l'attribut direction.
        /// </summary>
        public Directions Direction
        {
            get { return this.direction; }
            set { this.direction = value; }
        }

        /// <summary>
        /// Accesseur pour l'attribut direction.
        /// </summary>
        public float Echelle
        {
            get { return this.echelle; }
            set { this.echelle = value; }
        }

        /// <summary>
        /// Accesseur pour l'attribut direction.
        /// </summary>
        public int ContTombe
        {
            get { return this.contTombe; }
            set { this.contTombe = value; }
        }

        /// <summary>
        /// Accesseur pour l'attribut direction.
        /// </summary>
        public float VitesseVerticale
        {
            get { return 0.3f; }
            set { this.vitesseVerticale = value; }
        }

        /// <summary>
        /// Accesseur pour l'attribut direction.
        /// </summary>
        public float VitesseHorizontal
        {
            get { return 0.3f; }
            set { this.vitesseHorizontal = value; }
        }

        /// <summary>
        /// Accesseur pour la possession du clef.
        /// </summary>
        public bool Clef
        {
            get { return this.clef; }
            set { this.clef = value; }
        }

        /// <summary>
        /// Accesseur pour l'attribut etat.
        /// </summary>
        public Etats Etat
        {
            get 
            {
                return this.etat;   
            }

            // Le setter modifie les attributs (hérités) d'animation du sprite afin que les tuiles d'animation
            // correspondant au nouvel état du personnage soient exploitées.
            set 
            {
                bool resetBruit = this.etat != value;     // change-t-on d'état?

                // Si l'état change, arrêter le bruit de fond actif (s'il y en a un).
                if (resetBruit && this.bruitActif != null)
                {
                    this.bruitActif.Stop();
                }

                this.etat = value;      // enregistrer le nouvel état.

                // Sélectionner et paramétrer le bruitage de fond correspondant à l'état de déplacement.
                if (resetBruit)
                {
                    // Paramétrer le nouveau bruit de fond.
                    if (this.EffetsSonores != null && this.EffetsSonores[(int)this.etat] != null)
                    {
                        this.bruitActif = this.EffetsSonores[(int)this.etat].CreateInstance();
                        if (this.etat != Etats.Tombe)
                        {
                            this.bruitActif.IsLooped = true;
                            this.bruitActif.Volume = 1.0f;
                        }
                        else
                        {
                            this.bruitActif.Volume = 0.4f;
                            this.bruitActif.IsLooped = false;
                        }
                    }
                    else
                    {
                        this.bruitActif = null;
                    }
                }
            }
        }

        /// <summary>
        /// Propriété (accesseur de lecture seulement) retournant la position des pattes du sprite.
        /// Cette position est utilisée pour déterminer si le sprite est debout sur une tuile solide.
        /// </summary>
        public Vector2 PositionPourCollisions
        {
            get
            {
                int dx = 0, dy = 20;

                // La position considérée est celle des pattes devant le personnage,
                // ce qui dépend de la direction de déplacement
                if (this.etat != Etats.Stationnaire)
                {
                    if (this.direction == Directions.Est ||
                        this.direction == Directions.NordEst ||
                        this.direction == Directions.SudEst)
                    {
                        dx = +12;
                    }

                    else if (this.direction == Directions.Ouest ||
                        this.direction == Directions.NordOuest ||
                        this.direction == Directions.SudOuest)
                    {
                        dx = -12;
                    }

                    if (this.direction == Directions.Nord ||
                        this.direction == Directions.NordEst ||
                        this.direction == Directions.NordOuest)
                    {
                        dy -= 12;
                    }
                }
                return new Vector2(this.Position.X + dx, this.Position.Y + dy);
            }
        }

        /// <summary>
        /// Attribut représentant la camera.
        /// </summary>
        public Camera Camera
        {
            get { return this.camera; }
            set { this.camera = value; }
        }

        /// <summary>
        /// Propriété (accesseur pour getResistanceAuMouvement) retournant ou changeant la fonction déléguée 
        /// de calcul de résistance aux déplacements.
        /// </summary>
        /// <value>Fonction de calcul de résistance aux déplacements.</value>
        public ResistanceAuMouvement GetResistanceAuMouvement
        {
            get { return this.getResistanceAuMouvement; }
            set { this.getResistanceAuMouvement = value; }
        }

        /// <summary>
        /// Propriété (accesseur pour getValiderDeplacement) retournant ou changeant la fonction déléguée 
        /// de validation des déplacements.
        /// </summary>
        /// <value>Fonction de calcul de résistance aux déplacements.</value>
        public ValiderDeplacement GetValiderDeplacement
        {
            get { return this.getValiderDeplacement; }
            set { this.getValiderDeplacement = value; }
        }

        /// <summary>
        /// Propriété accesseur retournant la liste des palettes associées au personnage 
        /// selon son état et sa direction. Cette propriété doit obligatoirement être 
        /// surchargée dans les classes dérivées devant être instanciées.
        /// </summary>
        protected abstract List<PaletteTuiles> Palettes
        {
            get;
        }

        /// <summary>
        /// Propriété accesseur retournant la liste des effets sonores associée au personnage
        /// selon son état. Cette propriété doit obligatoirement être surchargée dans les 
        /// classes dérivées devant être instanciées.
        /// </summary>
        protected abstract List<SoundEffect> EffetsSonores
        {
            get;
        }

        /// <summary>
        /// Lire de  l'input les vitesses de déplacement directionnels.
        /// </summary>
        /// <param name="gameTime">Indique le temps écoulé depuis la dernière invocation.</param>
        /// <param name="vitesseNord">Retourne la vitesse de déplacement vers le nord.</param>
        /// <param name="vitesseSud">Retourne la vitesse de déplacement vers le sud.</param>
        /// <param name="vitesseEst">Retourne la vitesse de déplacement vers le est.</param>
        /// <param name="vitesseOuest">Retourne la vitesse de déplacement vers le ouest.</param>
        /// <returns>Vrai si des vitesses furent lues; faux sinon.</returns>
        public abstract bool LireVitesses(
            GameTime gameTime,
            out float vitesseNord,
            out float vitesseSud,
            out float vitesseEst,
            out float vitesseOuest);

        /// <summary>
        /// Ajuste la direction du sprite (propriété DirectionDeplacement) afin que ce dernier 
        /// regarde vers celle-ci.
        /// </summary>
        /// <param name="position">Les coordonnées (dans le monde) du point vers où orienter le sprite.</param>
        public void SeTournerVers(Vector2 position)
        {
            const float Pi = 3.14159265f;

            // Calculer l'angle de rotation exacte vers la destination. L'angle obtenu ci-dessus est 
            // dans l'interval -180 à 180 degrés, où 0 degré correspond au nord.
            float value, angle;
            value = (float)Math.Atan2(position.Y - this.Position.Y, position.X - this.Position.X);
            angle = (float)((Math.Atan2(position.Y - this.Position.Y, position.X - this.Position.X) * (180 / Pi)) + 90);

            // Le plan cartésien est divisé en 8 cadrans, chacun correspondant à une des dérections
            // de l'enum Directions. Aujster la direction de this en fonction du cadran correspondant 
            // à l'angle calculée.
            if (angle > -157.5f && angle <= -112.5f)
            {
                this.Direction = Directions.Ouest;
            }
            else if (angle > -112.5f && angle <= -67.5f)
            {
                this.Direction = Directions.Ouest;
            }
            else if (angle > -67.5f && angle <= -22.5f)
            {
                this.Direction = Directions.NordOuest;
            }
            else if (angle > -22.5f && angle <= 22.5f)
            {
                this.Direction = Directions.Nord;
            }
            else if (angle > 22.5f && angle <= 67.5f)
            {
                this.Direction = Directions.NordEst;
            }
            else if (angle > 67.5f && angle <= 112.5f)
            {
                this.Direction = Directions.Est;
            }
            else if (angle > 112.5f && angle <= 157.5f)
            {
                this.Direction = Directions.SudEst;
            }
            else if (angle > 112.5f && angle <= 157.5f)
            {
                this.Direction = Directions.SudEst;
            }
            else
            {
                this.Direction = Directions.Sud;
            }
        }

        /// <summary>
        /// Ajuste la position du sprite en fonction de l'input.
        /// </summary>
        /// <param name="gameTime">Gestionnaire de temps de jeu.</param>
        /// <param name="graphics">Gestionnaire de périphérique d'affichage.</param>
        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {            
            if (this.etat == Etats.Tombe)                                                      
            {
                this.angleRotation += gameTime.ElapsedGameTime.Milliseconds * 0.004f;
                this.angleRotation %= MathHelper.Pi * 2;
                this.contTombe++;
                this.echelle -= 0.015f;
                base.Update(gameTime, graphics);
                return;
            }

            // Obtenir les vitesses de déplacements (toutes entre 0.0 et 1.0) de l'input.
            float vitesseNord, vitesseSud, vitesseEst, vitesseOuest;
            if (!this.LireVitesses(gameTime, out vitesseNord, out vitesseSud, out vitesseEst, out vitesseOuest))
            {
                base.Update(gameTime, graphics);
                return;
            }

            // Éviter les directions contradictoires.
            if (vitesseNord > 0.0)
            {
                vitesseSud = 0.0f;
            }

            if (vitesseOuest > 0.0)
            {
                vitesseEst = 0.0f;
            }

            // Changer le sprite selon la direction.
            if (vitesseNord > 0.0 && vitesseEst > 0.0)
            {
                this.direction = Directions.NordEst;
            }
            else if (vitesseNord > 0.0 && vitesseOuest > 0.0)
            {
                this.direction = Directions.NordOuest;
            }
            else if (vitesseSud > 0.0 && vitesseEst > 0.0)
            {
                this.direction = Directions.SudEst;
            }
            else if (vitesseSud > 0.0 && vitesseOuest > 0.0)
            {
                this.direction = Directions.SudOuest;
            }
            else if (vitesseNord > 0.0)
            {
                this.direction = Directions.Nord;
            }
            else if (vitesseEst > 0.0)
            {
                this.direction = Directions.Est;
            }
            else if (vitesseOuest > 0.0)
            {
                this.direction = Directions.Ouest;
            }
            else if (vitesseSud > 0.0)
            {
                this.direction = Directions.Sud;
            }

            // Calcul de la vitesse de déplacement du personnage en fonction de l'input.
            float vitesse = this.vitesseMaximum;

            if (this.direction == Directions.Nord)
            {
                vitesse *= vitesseNord;
            }
            else if (this.direction == Directions.Sud)
            {
                vitesse *= vitesseSud;
            }
            else if (this.direction == Directions.Est)
            {
                vitesse *= vitesseEst;
            }
            else if (this.direction == Directions.Ouest)
            {
                vitesse *= vitesseOuest;
            }
            else if (this.direction == Directions.NordEst)
            {
                vitesse *= Math.Max(vitesseNord, vitesseEst);
            }
            else if (this.direction == Directions.NordOuest)
            {
                vitesse *= Math.Max(vitesseNord, vitesseOuest);
            }
            else if (this.direction == Directions.SudEst)
            {
                vitesse *= Math.Max(vitesseSud, vitesseEst);
            }
            else if (this.direction == Directions.SudOuest)
            {
                vitesse *= Math.Max(vitesseSud, vitesseOuest);
            }

            // Mettre à jour l'état du personnage selon sa vitesse de déplacement.
            if (vitesse >= 0.6 * this.vitesseMaximum)
            {
                this.Etat = Etats.Course;
            }
            else if (vitesse > 0.01)
            {
                this.Etat = Etats.Marche;
            }
            else
            {
                this.Etat = Etats.Stationnaire;
            }

            // Rendre la vitesse indépendante du matériel.
            vitesse *= gameTime.ElapsedGameTime.Milliseconds;

            // Calculer le déplacement du sprite selon la direction indiquée. Notez que
            // deux directions opposées s'annulent.
            int deltaX = 0, deltaY = 0;

            if (vitesseOuest > 0.0)
            {
                deltaX = (int)-vitesse;
            }

            if (vitesseEst > 0.0)
            {
                deltaX = (int)vitesse;
            }

            if (vitesseNord > 0.0)
            {
                deltaY = (int)-vitesse;
            }

            if (vitesseSud > 0.0)
            {
                deltaY = (int)vitesse;
            }

            // Si une fonction déléguée est fournie pour valider les mouvements sur les tuiles
            // y faire appel pour valider la position résultante du mouvement.
            if (this.getValiderDeplacement != null && (deltaX != 0.0 || deltaY != 0.0))
            {
                // Déterminer le déplacement maximal permis vers la nouvelle position en fonction
                // de la résistance des tuiles. Une résistance maximale de 0.95 est indiquée afin de
                // permettre au sprite de traverser les tuiles n'étant pas complètement solides.
                this.getValiderDeplacement(this.PositionPourCollisions, ref deltaX, ref deltaY, 0.95f);
            }

            // Si une fonction déléguée est fournie pour autoriser les mouvements sur les tuiles
            // y faire appel pour valider la position résultante du mouvement.
            if (this.getResistanceAuMouvement != null && (deltaX != 0.0 || deltaY != 0.0))
            {
                // Déterminer les coordonnées de destination et tenant compte que le sprite est
                // centré sur Position, alors que ses mouvements doivent être autorisés en fonction
                // de la position de ses pieds.
                Vector2 newPos = this.PositionPourCollisions;
                newPos.X += deltaX;
                newPos.Y += deltaY;

                // Calculer la résistance à la position du sprite.
                float resistance = this.getResistanceAuMouvement(newPos);

                // Appliquer le facteur de résistance obtenu au déplacement.
                deltaX = (int)(deltaX * (1.0f - resistance));
                deltaY = (int)(deltaY * (1.0f - resistance));
            }

            // Activer les effets sonores associés au personnage lorsque ceux-ci sont actifs.
            if (this.bruitActif != null)
            {
                if (this.bruitActif.State != SoundState.Playing)
                {
                    this.bruitActif.Play();
                    if (this.etat == Etats.Tombe)
                    {
                        this.bruitActif = null;
                    }
                }
            }

            // Modifier la position du sprite en conséquence (on exploite le setter
            // de _position afin d'appliquer boundsRect).
            this.Position = new Vector2(this.Position.X + deltaX, this.Position.Y + deltaY);

            // La fonction de base s'occupe de l'animation.
            base.Update(gameTime, graphics);
        }

        /// <summary>
        /// Suspend temporairement (pause) ou réactive les effets sonores du personnage.
        /// </summary>
        /// <param name="suspendre">Indique si les effets sonores doivent être suspendus ou réactivés.</param>
        public void SuspendreEffetsSonores(bool suspendre)
        {
            // Premièrement s'assurer qu'on a un bruit de fond actif.
            if (this.bruitActif == null)
            {
                return;
            }

            // On en a un, alors le suspendre ou le réactiver, selon l'argument.
            if (suspendre)
            {
                // Suspendre au besoin les effets sonores associés au personnage.
                if (this.bruitActif.State == SoundState.Playing)
                {
                    this.bruitActif.Pause();
                }
            }
            else
            {
                // Réactiver au besoin les effets sonores associés aux moteurs
                if (this.bruitActif.State == SoundState.Paused)
                {
                    this.bruitActif.Play();
                }
            }
        }

        /// <summary>
        /// Charge les images associées au sprite du personnage. Cette fonction invoque sa
        /// surcharge plus générale en lui fournissant des arguments vides (null et string.Empty) 
        /// comme paramètres d'effets sonores.
        /// Il faut invoquer cette fonction pour charger les palettes des classes dérivées
        /// n'exploitant pas d'effets sonores pour leur personnage.
        /// </summary>
        /// <param name="content">Gestionnaire de contenu permettant de charger les images du vaisseau.</param>
        /// <param name="graphics">Gestionanire de périphérique d'affichage permettant d'extraire
        /// les caractéristiques de celui-ci (p.ex. l'écran).</param>
        /// <param name="palettes">Liste où doivent être stockées les palettes chargées.</param>
        /// <param name="largeurTuiles">Largeur (en pixels) de chaque tuile dans les palettes chargées.</param>
        /// <param name="hauteurTuiles">Hauteur (en pixels) de chaque tuile dans les palettes chargées.</param>
        /// <param name="repertoirePalettes">Sous-répertoire de Content qui contient les répertoires
        /// contenant les palettes selon l'état du personnage.</param>
        protected static void LoadContent(
            ContentManager content,
            GraphicsDeviceManager graphics,
            List<PaletteTuiles> palettes,
            int largeurTuiles,
            int hauteurTuiles,
            string repertoirePalettes)
        {
            LoadContent(content, graphics, palettes, null, largeurTuiles, hauteurTuiles, repertoirePalettes, string.Empty);
        }

        /// <summary>
        /// Charge les images et les effets sonores associés au sprite du personnage.
        /// Il faut invoquer cette fonction dans LoadContent des classes dérivées pour 
        /// charger leurs palettes et effets sonores.
        /// </summary>
        /// <param name="content">Gestionnaire de contenu permettant de charger les images du vaisseau.</param>
        /// <param name="graphics">Gestionanire de périphérique d'affichage permettant d'extraire
        /// les caractéristiques de celui-ci (p.ex. l'écran).</param>
        /// <param name="palettes">Liste où doivent être stockées les palettes chargées.</param>
        /// <param name="effetsSonores">Liste où sont stockées les effets sonores chargés.</param>
        /// <param name="largeurTuiles">Largeur (en pixels) de chaque tuile dans les palettes chargées.</param>
        /// <param name="hauteurTuiles">Hauteur (en pixels) de chaque tuile dans les palettes chargées.</param>
        /// <param name="repertoirePalettes">Sous-répertoire de Content qui contient les répertoires
        /// contenant les palettes selon l'état du personnage.</param>
        /// <param name="repertoireEffetsSonores">Sous-répertoire de Content qui contient les effets 
        /// sonores selon l'état du personnage.</param>
        protected static void LoadContent(
            ContentManager content,
            GraphicsDeviceManager graphics,
            List<PaletteTuiles> palettes,
            List<SoundEffect> effetsSonores,
            int largeurTuiles,
            int hauteurTuiles,
            string repertoirePalettes,
            string repertoireEffetsSonores)
        {
            // Puisque les palettes sont répertoriées selon l'état, on procède ainsi,
            // chargeant les huit palettes directionnelles un état à la fois.
            foreach (Etats etat in Enum.GetValues(typeof(Etats)))
            {
                // Déterminer le répertoire contenant les palettes selon l'état, ainsi que
                // les effets sonores (sans égard à l'état).
                string repertoireEtat;     // répertoire des palettes
                switch (etat)
                {
                    case Etats.Marche:
                        repertoireEtat = "\\Marche";
                        break;
                    case Etats.Course:
                        repertoireEtat = "\\Course";
                        break;
                    case Etats.Tombe:
                        repertoireEtat = "\\Tombe";
                        break;
                    case Etats.Mort:
                    repertoireEtat = "\\Mort";
                    break;
                    default:
                        repertoireEtat = "\\Stationnaire";
                        break;
                }

                // Charger les différentes palettes du personnage selon les directions. Notez que le
                // répertoire fourni doit OBLIGATOIREMENT contenir une palette pour chaque direction
                // de déplacement, et ce pour chaque état.
                string repertoire = repertoirePalettes + repertoireEtat;
                palettes.Add(new PaletteTuiles(content.Load<Texture2D>(repertoire + "\\Nord"), largeurTuiles, hauteurTuiles));
                palettes.Add(new PaletteTuiles(content.Load<Texture2D>(repertoire + "\\NordEst"), largeurTuiles, hauteurTuiles));
                palettes.Add(new PaletteTuiles(content.Load<Texture2D>(repertoire + "\\Est"), largeurTuiles, hauteurTuiles));
                palettes.Add(new PaletteTuiles(content.Load<Texture2D>(repertoire + "\\SudEst"), largeurTuiles, hauteurTuiles));
                palettes.Add(new PaletteTuiles(content.Load<Texture2D>(repertoire + "\\Sud"), largeurTuiles, hauteurTuiles));
                palettes.Add(new PaletteTuiles(content.Load<Texture2D>(repertoire + "\\SudOuest"), largeurTuiles, hauteurTuiles));
                palettes.Add(new PaletteTuiles(content.Load<Texture2D>(repertoire + "\\Ouest"), largeurTuiles, hauteurTuiles));
                palettes.Add(new PaletteTuiles(content.Load<Texture2D>(repertoire + "\\NordOuest"), largeurTuiles, hauteurTuiles));

                // Charger les bruitages de fond du personnage pour différents états. On utilise
                // un try-catch car il n'est pas requis par la classe dérivée de fournir un effet
                // sonore pour chaque état.
                if (effetsSonores != null)
                {
                    try
                    {
                        effetsSonores.Add(content.Load<SoundEffect>(repertoireEffetsSonores + repertoireEtat));
                    }
                    catch (ContentLoadException)
                    {
                        // Ajouter null à la liste pour indiquer que cet état ne dispose pas
                        // d'effet sonore associé.
                        effetsSonores.Add(null);
                    }
                }
            }
        }
    }
}
