//-----------------------------------------------------------------------
// <copyright file="JoueurSprite.cs" company="Marco Lavoie">
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
    public delegate void ValiderDeplacement1(Vector2 posSource, ref int deltaX, ref int deltaY, float resistanceMax);

    /// <summary>
    /// Classe implantant le sprite représentant le soldat contrôlé par le joueur. Ce sprite
    /// animé peut être stationnaire, marcher et courir dans huit directions.
    /// </summary>
    public class Projectile : SpriteAnimation
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

        private TypesProjectiles typeProjectile;

        public enum TypesProjectiles
        {

            Joueur,

            Ennemi
        }

        public TypesProjectiles TypeProjectile
        {
            get { return this.typeProjectile; }
            set { this.typeProjectile = value; }
        }

        /// <summary>
        /// Attribut statique (i.e. partagé par toutes les instances) constituant une 
        /// liste de palettes à exploiter selon la direction et l'état du joueur.
        /// </summary>
        private static PaletteTuiles palettes;

        /// <summary>
        /// Effet sonore contenant le bruitage du joueur en état de marche.
        /// </summary>
        private static SoundEffect bruitLaser;

        /// <summary>
        /// Effet sonore contenant le bruitage du joueur en état de course.
        /// </summary>
        private static SoundEffect bruitFrapper;      

        /// <summary>
        /// Vitesse de marche du joueur, avec valeur par défaut.
        /// </summary>
        private float VitesseHorizontale = 0.0f;

        public float vitesHorizontale
        {
            get { return  this.VitesseHorizontale; }
            set { this.VitesseHorizontale += value; }
        }

        private float VitesseVerticale = 0.0f;
        /// <summary>
        /// Vitesse de marche du joueur, avec valeur par défaut.
        /// </summary>
        public float vitesseVerticale
        {
            get { return this.VitesseVerticale; }
            set { this.VitesseVerticale += value; }
        }

        /// <summary>
        /// Attribut indiquant la direction de déplacement courante.
        /// </summary>
        private Direction direction;

        // =======================================================================================================
        private float vieDeProjectile = 5f;

        public float VideDeProjectile
        {
            get { return this.vieDeProjectile; }
            set { this.vieDeProjectile = value; }
        }

        private float animProjectile = 5f;

        public float AnimProjectile
        {
            get { return this.animProjectile; }
            set { this.animProjectile = value; }
        }
        

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite.
        /// </summary>
        /// <param name="x">Coordonnée initiale x (horizontale) du sprite.</param>
        /// <param name="y">Coordonnée initiale y (verticale) du sprite.</param>
        public Projectile(float x, float y)
            : base(x, y)
        {
 
        }

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite. On invoque l'autre constructeur.
        /// </summary>
        /// <param name="position">Coordonnées initiales horizontale et verticale du sprite.</param>
        public Projectile(Vector2 position, int direction)
            : this(position.X, position.Y)
        {
            switch (direction)
            {
                case 0 :
                    this.direction = Direction.Nord;
                    VitesseHorizontale = 0.0f;
                    vitesseVerticale = -0.5f;
                    break;

                case 1:
                    this.direction = Direction.NordEst;
                    VitesseHorizontale = 0.5f;
                    vitesseVerticale = -0.5f;
                    break;

                case 2:
                    this.direction = Direction.Est;
                    VitesseHorizontale = +0.5f;
                    vitesseVerticale = 0.0f;
                    break;

                case 3:
                    this.direction = Direction.SudEst;
                    VitesseHorizontale = 0.5f;
                    vitesseVerticale = 0.5f;
                    break;

                case 4:
                    this.direction = Direction.Sud;
                    VitesseHorizontale = 0.0f;
                    vitesseVerticale = 0.5f;
                    break;

                case 5:
                    this.direction = Direction.SudOuest;
                    VitesseHorizontale = 0.5f;
                    vitesseVerticale = -0.5f;
                    break;

                case 6:
                    this.direction = Direction.Ouest;
                    VitesseHorizontale = -0.5f;
                    vitesseVerticale = 0.0f;
                    break;

                case 7:
                    this.direction = Direction.NordOuest;
                    VitesseHorizontale = -0.5f;
                    vitesseVerticale = -0.5f;
                    break;

                default:
                    this.direction = Direction.Nord;
                    VitesseHorizontale = 0.0f;
                    vitesseVerticale = -0.5f;
                    break;
            }

            bruitLaser.Play();
            
        }

        /// <summary>
        /// Enumération des directions potentielles de déplacement du joueur.
        /// </summary>
        public enum Direction
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
        /// Accesseur pour attribut vitesseMaximum.
        /// </summary>
        public float VitesseMaximum
        {
            get { return this.VitesseMaximum; }
            set { this.VitesseMaximum = value; }
        }

        protected override PaletteTuiles Palette
        {
            // Les palettes sont stockées dans la liste en groupes d'état (i.e.
            // 8 palettes de direction pour chaque état).
            get { return palettes; }
        }

        /// <summary>
        /// Propriété (accesseur de lecture seulement) retournant la position des pattes du sprite.
        /// Cette position est utilisée pour déterminer si le sprite est debout sur une tuile solide.
        /// </summary>
        public Vector2 PositionPourCollisions
        {
            get
            {
                int dx = 0, dy = (Height / 2) - 20;

                // La position considérée est celle des pattes devant le personnage,
                // ce qui dépend de la direction de déplacement
                if (this.direction == Direction.Est ||
                    this.direction == Direction.NordEst ||
                    this.direction == Direction.SudEst)
                {
                    dx += (Width / 2) - 30;
                }
                else if (this.direction == Direction.Ouest ||
                    this.direction == Direction.NordOuest ||
                    this.direction == Direction.SudOuest)
                {
                    dx -= (Width / 2) - 30;
                }

                return new Vector2(this.Position.X + dx, this.Position.Y + dy);
            }
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
        /// Charge les images associées au sprite du joueur.
        /// </summary>
        /// <param name="content">Gestionnaire de contenu permettant de charger les images du vaisseau.</param>
        /// <param name="graphics">Gestionanire de périphérique d'affichage permettant d'extraire
        /// les caractéristiques de celui-ci (p.ex. l'écran).</param>
        public static void LoadContent(ContentManager content, GraphicsDeviceManager graphics)
        {
            palettes = new PaletteTuiles(content.Load<Texture2D>("Textures\\Joueur\\PH_Fireball"), 27, 27);

            // Charger les bruitages de fond du joueur pour différents états
            bruitLaser = content.Load<SoundEffect>("Audio\\Effets\\Projectile\\Projectile");
           
            
            bruitFrapper = content.Load<SoundEffect>("Audio\\Effets\\Joueur\\Course");
        }

        /// <summary>
        /// Ajuste la position du sprite en fonction de l'input.
        /// </summary>
        /// <param name="gameTime">Gestionnaire de temps de jeu.</param>
        /// <param name="graphics">Gestionnaire de périphérique d'affichage.</param>
        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            ForcerPosition(Position.X + (gameTime.ElapsedGameTime.Milliseconds * this.VitesseHorizontale),
                Position.Y + (gameTime.ElapsedGameTime.Milliseconds * this.vitesseVerticale));

            this.VideDeProjectile -= 0.1f;

            animProjectile -= .01f;
            //animProjectile = (float)Math.Sin(this.animProjectile);

            // La fonction de base s'occupe de l'animation.
            base.Update(gameTime, graphics);
        }

        /// <summary>
        /// Affiche à l'écran le sprite en fonction de la position de la camera. L'affichage est
        /// déléguée à palette afin d'afficher la tuile courante d'animation.
        /// </summary>
        /// <param name="camera">Caméra à exploiter pour l'affichage.</param>
        /// <param name="spriteBatch">Gestionnaire d'affichage en batch aux périphériques.</param>
        public override void Draw(Camera camera, SpriteBatch spriteBatch)
        {
            // Comme l'attribut _position contient la position centrée du sprite mais
            // que Draw() considère la position fournie comme celle de l'origine du
            // sprite, il faut décaler _position en conséquence avant d'invoquer Draw().
            ForcerPosition(Position.X - (this.Width / 2), Position.Y - (this.Height / 2));

            // Créer destRect aux coordonnées du sprite dans le monde. À noter que
            // les dimensions de destRect sont constantes.
            Rectangle destRect = new Rectangle((int)Position.X, (int)Position.Y, this.Width, this.Height);

            // Afficher le sprite s'il est visible.
            if (camera == null)
            {
                // Afficher la tuile courante.
                this.Palette.Draw((int)this.IndexTuile, destRect, spriteBatch);
            }
            else if (camera.EstVisible(destRect))
            {
                // Puisque le sprite est visible, déléguer à la palette de tuiles la tâche d'afficher
                // la tuile courante.

                // Décaler la destination en fonction de la caméra. Ceci correspond à transformer destRect 
                // de coordonnées logiques (i.e. du monde) à des coordonnées physiques (i.e. de l'écran).
                camera.Monde2Camera(ref destRect);

                // Afficher la tuile courante.
                this.Palette.Draw((int)this.IndexTuile, destRect, spriteBatch);
            }

            // Remettre _position au centre du sprite.
            ForcerPosition(Position.X + (this.Width / 2), Position.Y + (this.Height / 2));
        }
    }
}
