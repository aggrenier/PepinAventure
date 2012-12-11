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
    public delegate float ResistanceAuMouvement2(Vector2 position);

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
    public delegate void ValiderDeplacement2(Vector2 posSource, ref float deltaX, ref float deltaY, float resistanceMax);

    /// <summary>
    /// Classe implantant le sprite représentant le soldat contrôlé par le joueur. Ce sprite
    /// animé peut être stationnaire, marcher et courir dans huit directions.
    /// </summary>
    public class Bloc : SpriteAnimation
    {
        /// <summary>
        /// Fonction déléguée permettant d'obtenir la résistance aux déplacements du sprite
        /// dans le monde de tuiles. Si aucune fonction déléguée n'est fournie, aucune
        /// résistance n'est appliquée aux déplacements.
        /// </summary>
        private ResistanceAuMouvement2 getResistanceAuMouvement;

        /// <summary>
        /// Fonction déléguée permettant de valider les déplacements du sprite
        /// dans le monde de tuiles. Si aucune fonction déléguée n'est fournie, aucune
        /// résistance n'est appliquée aux déplacements.
        /// </summary>
        private ValiderDeplacement2 getValiderDeplacement;


        /// <summary>
        /// Attribut statique (i.e. partagé par toutes les instances) constituant une 
        /// liste de palettes à exploiter selon la direction et l'état du joueur.
        /// </summary>
        private static PaletteTuiles palettes;

        /// <summary>
        /// Effet sonore contenant le bruitage du joueur en état de marche.
        /// </summary>
        private static SoundEffect bruitLancer;

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
            get { return this.VitesseHorizontale; }
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

       
        private float vieDeBloc = 2f;

        public float VideDeBloc
        {
            get { return this.vieDeBloc; }
            set { this.vieDeBloc = value; }
        }

        private bool blockMouvement = true;

        public bool BlockMouvement
        {
            get { return this.blockMouvement; }
            set { this.blockMouvement = value; }
        }


        /// <summary>
        /// Constructeur paramétré recevant la position du sprite.
        /// </summary>
        /// <param name="x">Coordonnée initiale x (horizontale) du sprite.</param>
        /// <param name="y">Coordonnée initiale y (verticale) du sprite.</param>
        public Bloc(float x, float y)
            : base(x, y)
        {

        }

        /// <summary>
        /// Accesseur pour attribut vitesseMaximum.
        /// </summary>
        public float VitesseMaximum
        {
            get { return this.VitesseMaximum; }
            set { this.VitesseMaximum = value; }
        }

        /// <summary>
        /// Accesseur pour la palette.
        /// </summary>
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
               return new Vector2(this.Position.X , this.Position.Y );
            }
        }
              

        /// <summary>
        /// Propriété (accesseur pour getResistanceAuMouvement) retournant ou changeant la fonction déléguée 
        /// de calcul de résistance aux déplacements.
        /// </summary>
        /// <value>Fonction de calcul de résistance aux déplacements.</value>
        public ResistanceAuMouvement2 GetResistanceAuMouvement
        {
            get { return this.getResistanceAuMouvement; }
            set { this.getResistanceAuMouvement = value; }
        }

        /// <summary>
        /// Propriété (accesseur pour getValiderDeplacement) retournant ou changeant la fonction déléguée 
        /// de validation des déplacements.
        /// </summary>
        /// <value>Fonction de calcul de résistance aux déplacements.</value>
        public ValiderDeplacement2 GetValiderDeplacement
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
            palettes = new PaletteTuiles(content.Load<Texture2D>("Objects\\Bloc"), 28, 28);
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

            if(this.vitesHorizontale != 0 || this.vitesseVerticale != 0)
            this.VideDeBloc -= 0.05f;


            //// Calculer le déplacement du sprite selon la direction indiquée. Notez que
            //// deux directions opposées s'annulent.
            //float deltaX = vitesHorizontale;
            //float deltaY = vitesseVerticale;            

            //// Si une fonction déléguée est fournie pour valider les mouvements sur les tuiles
            //// y faire appel pour valider la position résultante du mouvement.
            //if (this.getValiderDeplacement != null && (deltaX != 0.0 || deltaY != 0.0))
            //{
            //    // Déterminer le déplacement maximal permis vers la nouvelle position en fonction
            //    // de la résistance des tuiles. Une résistance maximale de 0.95 est indiquée afin de
            //    // permettre au sprite de traverser les tuiles n'étant pas complètement solides.
            //    this.getValiderDeplacement(this.PositionPourCollisions, ref deltaX, ref deltaY, 0.95f);
            //}

            //// Si une fonction déléguée est fournie pour autoriser les mouvements sur les tuiles
            //// y faire appel pour valider la position résultante du mouvement.
            //if (this.getResistanceAuMouvement != null && (deltaX != 0.0 || deltaY != 0.0))
            //{
            //    // Déterminer les coordonnées de destination et tenant compte que le sprite est
            //    // centré sur Position, alors que ses mouvements doivent être autorisés en fonction
            //    // de la position de ses pieds.
            //    Vector2 newPos = this.PositionPourCollisions;
            //    newPos.X += deltaX;
            //    newPos.Y += deltaY;

            //    // Calculer la résistance à la position du sprite.
            //    float resistance = this.getResistanceAuMouvement(newPos);

            //    // Appliquer le facteur de résistance obtenu au déplacement.
            //    deltaX = (int)(deltaX * (1.0f - resistance));
            //    deltaY = (int)(deltaY * (1.0f - resistance));
            //}           

            //// Modifier la position du sprite en conséquence (on exploite le setter
            //// de _position afin d'appliquer boundsRect).
            //this.Position = new Vector2(this.Position.X + deltaX, this.Position.Y + deltaY);


            // La fonction de base s'occupe de l'animation.
            base.Update(gameTime, graphics);
        }
    }
}
