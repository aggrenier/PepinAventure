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
    /// Classe implantant le sprite représentant le soldat contrôlé par le joueur. Ce sprite
    /// animé peut être stationnaire, marcher et courir dans huit directions.
    /// </summary>
    public class Porte : SpriteAnimation
    {
        /// <summary>
        /// Attribut statique (i.e. partagé par toutes les instances) constituant une 
        /// liste de palettes à exploiter selon la direction et l'état du joueur.
        /// </summary>
        private static PaletteTuiles palettes;

        /// <summary>
        /// Effet sonore contenant le bruitage du joueur en état de marche.
        /// </summary>
        private static SoundEffect bruitOuvrir;

        /// <summary>
        /// Effet sonore contenant le bruitage du joueur en état de course.
        /// </summary>
        private static SoundEffect bruitFermer;

        private bool ouvert = false;

        public bool Ouvert
        {
            get { return this.ouvert; }
            set { 
                this.ouvert = value;
                if (this.ouvert)
                    IndexTuile = 0;
                else
                    IndexTuile = 2;
            }
        }

        private Rectangle barre;

        public Rectangle Barre
        {
            get { return this.barre; }
            set { this.barre = value; }
        }

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite.
        /// </summary>
        /// <param name="x">Coordonnée initiale x (horizontale) du sprite.</param>
        /// <param name="y">Coordonnée initiale y (verticale) du sprite.</param>
        public Porte(int x, int y, Directions direction)
            : base(x, y)
        {
            barre.X = x - Width/2;
            barre.Y = y - Height/2;
            barre.Width = Width;
            barre.Height = Height;
        }

        /// <summary>
        /// Attribut indiquant la direction de déplacement courante.
        /// </summary>
        private Directions direction;

        public Directions Direction
        {
            get { return this.direction; }
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
            /// Déplacement vers la gauche l'écran.
            /// </summary>
            Est,

            /// <summary>
            /// Déplacement vers le bas de l'écran.
            /// </summary>
            Sud,

            /// <summary>
            /// Déplacement vers la droite de l'écran.
            /// </summary>
            Ouest
        }

        /// <summary>
        /// Accesseur pour la palette.
        /// </summary>
        protected override PaletteTuiles Palette
        {
            // Les palettes sont stockées dans la liste en groupes d'état (i.e.
            // 4 palettes de direction pour chaque état).
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
                return new Vector2(this.Position.X, this.Position.Y);
            }
        }


        /// <summary>
        /// Charge les images associées au sprite du joueur.
        /// </summary>
        /// <param name="content">Gestionnaire de contenu permettant de charger les images du vaisseau.</param>
        /// <param name="graphics">Gestionanire de périphérique d'affichage permettant d'extraire
        /// les caractéristiques de celui-ci (p.ex. l'écran).</param>
        public static void LoadContent(ContentManager content, GraphicsDeviceManager graphics)
        {

            palettes = new PaletteTuiles(content.Load<Texture2D>("Textures\\Porte\\Nord"), 48, 36);
            //palettes = new PaletteTuiles(content.Load<Texture2D>("Textures\\Porte\\Sud"), 48, 36);
            //palettes.Add(new PaletteTuiles(content.Load<Texture2D>("Textures\\Porte\\Est"), 32, 48));
            //palettes = new PaletteTuiles(content.Load<Texture2D>("Textures\\Porte\\Ouest"), 36, 28);

        }

        /// <summary>
        /// Ajuste la position du sprite en fonction de l'input.
        /// </summary>
        /// <param name="gameTime">Gestionnaire de temps de jeu.</param>
        /// <param name="graphics">Gestionnaire de périphérique d'affichage.</param>
        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {


            // La fonction de base s'occupe de l'animation.
            base.Update(gameTime, graphics);
        }
    }
}
