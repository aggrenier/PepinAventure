//-----------------------------------------------------------------------
// <copyright file="Joueur.cs" company="Marco Lavoie">
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

namespace Exercice_12_1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using IFM20884;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Classe implantant le sprite représentant le soldat contrôlé par le joueur. Ce sprite
    /// animé peut être stationnaire, marcher et courir dans huit directions.
    /// </summary>
    public class Joueur : Personnage
    {
        /// <summary>
        /// Attribut statique (i.e. partagé par toutes les instances) constituant une 
        /// liste de palettes à exploiter selon la direction et l'état du personnage.
        /// </summary>
        private static List<PaletteTuiles> palettes = new List<PaletteTuiles>();

        /// <summary>
        /// Effet sonore contenant le bruitage du personnage selon lson état.
        /// </summary>
        private static List<SoundEffect> effetsSonores = new List<SoundEffect>();

        /// <summary>
        /// Attribut indiquant l'index du périphérique contrôlant le sprite (voir
        /// dans Update (1 par défaut).
        /// </summary>
        private int indexPeripherique = 1;        

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite.
        /// </summary>
        /// <param name="x">Coordonnée initiale x (horizontale) du sprite.</param>
        /// <param name="y">Coordonnée initiale y (verticale) du sprite.</param>
        public Joueur(float x, float y)
            : base(x, y) 
        {
            this.VieDeJoueur = 5;
        }

        /// <summary>
        /// Utilisé pour pousser les blocs.
        /// </summary>
        private int contPousseBloc = 0;
        
        public int ContPousseBloc
        {
            get { return this.contPousseBloc; }
            set { this.contPousseBloc = value; }
        }

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite. On invoque l'autre constructeur.
        /// </summary>
        /// <param name="position">Coordonnées initiales horizontale et verticale du sprite.</param>
        public Joueur(Vector2 position)
            : this(position.X, position.Y)
        {
        }

        /// <summary>
        /// Propriété indiquant l'index du périphérique contrôlant le sprite (1 à 4).
        /// </summary>
        public int IndexPeripherique
        {
            get { return this.indexPeripherique; }
            set { this.indexPeripherique = Math.Min(Math.Max(value, 0), 4); }
        }

        //public override int IndexTuile
        //{
        //    get { return this.IndexTuile; }
        //    //set {
        //    //    if (!(this.Etat == Etats.Mort && this.IndexTuile == 4))
        //    //    {
        //    //        this.IndexTuile = value;
        //    //    }
        //    //    else
        //    //    {
        //    //        this.IndexTuile = 4;
        //    //    }
        //    //}
        //}

        /// <summary>
        /// Propriété accesseur retournant la liste des palettes associées au personnage 
        /// selon son état et sa direction. Ces palettes sont stockées dans l'attribut 
        /// static palettes.
        /// </summary>
        protected override List<PaletteTuiles> Palettes
        {
            get { return Joueur.palettes; }
        }

        /// <summary>
        /// Propriété accesseur retournant la liste des effets sonores associée au personnage
        /// selon son état. Ces effets sonores sont stockées dans l'attribut static 
        /// effetsSonores.
        /// </summary>
        protected override List<SoundEffect> EffetsSonores
        {
            get { return Joueur.effetsSonores; }
        }

        /// <summary>
        /// Surchargé afin de retourner la palette correspondant à la direction de 
        /// déplacement et l'état du personnage.
        /// </summary>
        protected override PaletteTuiles Palette
        {
            // Les palettes sont stockées dans la liste en groupes d'état (i.e.
            // 8 palettes de direction pour chaque état).
            get { return palettes[((int)this.Etat * 8) + (int)this.Direction]; }
        }

        /// <summary>
        /// Charge les images associées au sprite du joueur. Cette fonction static invoque
        /// la fonction static de la classe de base qui s'occupe de charger les textures
        /// et effets sonores que devraient avoir toute classe dérivée de Personnage.
        /// </summary>
        /// <param name="content">Gestionnaire de contenu permettant de charger les images du vaisseau.</param>
        /// <param name="graphics">Gestionanire de périphérique d'affichage permettant d'extraire
        /// les caractéristiques de celui-ci (p.ex. l'écran).</param>
        public static void LoadContent(ContentManager content, GraphicsDeviceManager graphics)
        {
            LoadContent(
                content,                    // gestionnaire de contenu à utiliser
                graphics,                   // gestionnaire de périphériques à utiliser
                Joueur.palettes,            // liste où doivent être stockées les palettes du joueur
                Joueur.effetsSonores,       // liste où doivent être stockés les effets sonores du joueur
                64,                         // largeur de chaque tuile dans les palettes
                64,                         // hauteur de chaque tuile dans les palettes
                "Textures\\Joueur",         // sous-répertoire de Content où sont stockées les palettes du joueur
                "Audio\\Effets\\Joueur");   // sous-répertoire de Content où sont stockées les effets sonores du joueur
           
            // Imposer la palette de collisions au déplacement du joueur. 
           // this.joueur.GetResistanceAuMouvement = this.CalculerResistanceAuMouvement;
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
        public override bool LireVitesses(
            GameTime gameTime,
            out float vitesseNord,
            out float vitesseSud,
            out float vitesseEst,
            out float vitesseOuest)
        {
            // Premièrement s'assurer qu'un service de lecture de périphérique d'inputs est
            // disponible
            if (ServiceHelper.Disponible<IInputService>() && this.VieDeJoueur != 0)
            {
                // Obtenir les vitesses de déplacements (toutes entre 0.0 et 1.0) de l'input
                vitesseNord = ServiceHelper.Get<IInputService>().DeplacementAvant(this.indexPeripherique);
                vitesseSud = ServiceHelper.Get<IInputService>().DeplacementArriere(this.indexPeripherique);
                vitesseEst = ServiceHelper.Get<IInputService>().DeplacementDroite(this.indexPeripherique);
                vitesseOuest = ServiceHelper.Get<IInputService>().DeplacementGauche(this.indexPeripherique);

                return true;
            }
            else
            {
                // Aucun périphérique d'inputs disponible, alors aucune vitesse lue
                vitesseNord = 0.0f;
                vitesseSud = 0.0f;
                vitesseEst = 0.0f;
                vitesseOuest = 0.0f;

                return false;
            }
        }
    }
}
