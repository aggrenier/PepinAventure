//-----------------------------------------------------------------------
// <copyright file="MondeArriere.cs" company="Marco Lavoie">
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
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Classe concrétisant un monde d'image de Mario Bros.
    /// </summary>
    public class MondeArriere : MondeTuiles
    {
        /// <summary>
        /// Mappe monde : chaque valeur du tableau correspond à l'index d'une tuile dans le monde.
        /// </summary>
        private static int[,] mappeMonde =
            {
                { 0, 1, 2, 3, 3, 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, }, 
                { 5, 6, 7, 3, 3, 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 8, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 10, }, 
                { 5, 11, 7, 3, 0, 12, 12, 12, 12, 12, 2, 4, 4, 4, 4, 4, 13, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 15, }, 
                { 5, 16, 7, 3, 5, 17, 17, 17, 17, 18, 19, 20, 20, 20, 20, 20, 21, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 15, }, 
                { 5, 17, 7, 3, 5, 17, 22, 23, 17, 17, 24, 25, 25, 25, 25, 25, 26, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 15, }, 
                { 5, 17, 19, 20, 27, 17, 28, 29, 30, 31, 32, 3, 33, 3, 3, 3, 13, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 15, }, 
                { 5, 17, 24, 25, 34, 17, 17, 17, 35, 3, 3, 3, 4, 3, 4, 3, 13, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 15, }, 
                { 5, 17, 7, 3, 36, 37, 37, 37, 32, 3, 3, 3, 4, 3, 3, 3, 13, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 15, }, 
                { 5, 17, 7, 3, 3, 3, 3, 3, 3, 4, 3, 33, 3, 3, 3, 3, 13, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 15, }, 
                { 5, 17, 7, 3, 3, 3, 3, 33, 33, 3, 3, 33, 3, 33, 3, 3, 13, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 15, }, 
                { 5, 17, 7, 3, 3, 3, 3, 3, 33, 3, 3, 33, 33, 3, 3, 3, 13, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 15, }, 
                { 5, 17, 7, 3, 33, 4, 3, 3, 33, 33, 3, 33, 33, 3, 3, 3, 38, 39, 39, 39, 39, 39, 39, 39, 39, 39, 39, 39, 39, 39, 39, 40, }, 
                { 5, 17, 7, 3, 3, 4, 33, 3, 4, 3, 3, 33, 33, 3, 3, 3, 3, 3, 3, 3, 3, 3, 41, 4, 41, 3, 3, 3, 3, 3, 3, 3, }, 
                { 5, 17, 7, 3, 3, 3, 3, 3, 3, 3, 3, 4, 3, 3, 3, 3, 33, 4, 4, 3, 3, 3, 4, 4, 4, 4, 3, 3, 3, 3, 33, 3, }, 
                { 5, 17, 7, 3, 33, 3, 3, 3, 3, 3, 3, 3, 33, 3, 3, 3, 3, 33, 3, 3, 3, 33, 4, 4, 4, 33, 3, 3, 3, 3, 3, 3, }, 
                { 5, 17, 7, 3, 3, 3, 4, 33, 33, 3, 3, 3, 3, 3, 3, 3, 4, 3, 3, 33, 3, 3, 4, 4, 4, 3, 3, 3, 3, 3, 3, 3, }, 
                { 5, 17, 7, 3, 3, 4, 4, 3, 3, 3, 3, 3, 3, 3, 33, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 3, 3, 3, 3, 3, 3, 3, }, 
                { 5, 17, 7, 3, 3, 4, 4, 3, 3, 3, 3, 3, 3, 3, 3, 4, 3, 3, 3, 3, 3, 3, 4, 4, 4, 33, 33, 3, 4, 4, 3, 3, }, 
                { 5, 17, 7, 3, 3, 3, 3, 3, 33, 3, 3, 33, 33, 3, 3, 3, 3, 3, 33, 33, 3, 3, 4, 4, 4, 3, 33, 33, 3, 4, 3, 3, }, 
                { 5, 17, 7, 3, 3, 3, 3, 3, 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 3, 33, 33, 3, 3, 3, 3, }, 
                { 5, 17, 42, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 43, 44, 44, 44, 44, 44, 44, 44, 44, 44, 44, 45, 12, 12, 2, 4, 4, 4, }, 
                { 5, 17, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 17, 17, 17, 17, 17, 17, 17, 18, 18, 18, 42, 12, 12, 12, }, 
                { 5, 17, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 17, 17, 17, 17, 17, 17, 17, 18, 18, 18, 18, 18, 18, 18, }, 
                { 5, 17, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 46, 47, 47, 47, 47, 48, 18, 18, 18, 18, 18, 18, 18, }, 
                { 5, 17, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 49, 50, 51, 52, 18, 18, 53, 54, 55, 56, 56, 57, 18, 18, 18, 18, 18, 18, 18, }, 
                { 5, 17, 18, 18, 18, 22, 58, 59, 59, 59, 58, 23, 18, 60, 61, 62, 63, 18, 18, 53, 64, 65, 56, 56, 56, 47, 47, 47, 47, 48, 18, 18, }, 
                { 5, 17, 18, 18, 18, 66, 67, 67, 67, 67, 67, 68, 18, 69, 70, 71, 72, 18, 18, 53, 56, 73, 56, 56, 56, 56, 56, 73, 56, 57, 18, 18, }, 
                { 5, 17, 18, 18, 18, 66, 67, 74, 75, 67, 67, 68, 18, 76, 77, 78, 79, 18, 18, 53, 56, 56, 56, 56, 56, 56, 56, 56, 56, 57, 18, 18, }, 
                { 80, 17, 18, 18, 18, 66, 67, 81, 82, 67, 67, 68, 18, 76, 77, 78, 79, 18, 18, 83, 84, 84, 56, 56, 85, 86, 56, 56, 56, 57, 18, 18, }, 
                { 87, 18, 18, 18, 18, 66, 67, 67, 67, 67, 67, 68, 18, 88, 89, 90, 91, 18, 18, 18, 18, 18, 53, 56, 56, 56, 56, 92, 93, 57, 18, 18, }, 
                { 94, 18, 18, 18, 18, 28, 95, 96, 95, 95, 95, 29, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 83, 84, 84, 84, 84, 84, 84, 97, 18, 18, }, 
                { 98, 99, 99, 100, 99, 101, 99, 100, 101, 100, 99, 99, 101, 99, 87, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, }
            };

        /// <summary>
        /// Palette de tuiles exploitée par le monde.
        /// </summary>
        private static PaletteTuiles paletteTuiles;

        /// <summary>
        /// Palette de collisions exploitée par le monde.
        /// </summary>
        private static PaletteTuiles paletteCollisions;

        /// <summary>
        /// Constructeur par défaut - rien à faire!
        /// </summary>
        public MondeArriere()
        {
        }

        /// <summary>
        /// Propriété (accesseur de paletteCollisions) retournant et changeant la palette de gestion des collisions
        /// de sprites avec les tuiles (peut être nul).
        /// </summary>
        /// <value>Palette de gestion des collisions de sprites avec les tuiles.</value>
        public override PaletteTuiles PaletteCollisions
        {
            get { return paletteCollisions; }
        }

        /// <summary>
        /// Accesseur à surcharger retournant la position initiale du sprite 
        /// du joueur dans le monde.
        /// </summary>
        public override Vector2 PositionInitiale
        {
            get { return new Vector2(182, 550); }
        }

        /// <summary>
        /// Propriété (accesseur de paletteCollisions) retournant et changeant le tableau d'index 
        /// des tuiles du monde.
        /// </summary>
        protected override int[,] MappeMonde
        {
            get { return mappeMonde; }
        }

        /// <summary>
        /// Propriété (accesseur de paletteCollisions) retournant et changeant la palette de tuiles 
        /// constituant le monde.
        /// </summary>
        protected override PaletteTuiles Palette
        {
            get { return paletteTuiles; }
        }

        /// <summary>
        /// Charge les images d'affichage et de détection de collisions.
        /// </summary>
        /// <param name="content">Gestionnaire de contenu permettant de charger les images du vaisseau.</param>
        /// <param name="graphics">Gestionanire de périphérique d'affichage permettant d'extraire
        /// les caractéristiques de celui-ci (p.ex. l'écran).</param>
        public static void LoadContent(ContentManager content, GraphicsDeviceManager graphics)
        {
            // Créer une palette de tuiles pour la mappe.
            paletteTuiles = new PaletteTuiles(content.Load<Texture2D>("Textures\\Monde\\TilesBack"), 48, 48);

            // Charger la palette de tuiles de détection de collisions.
            paletteCollisions = new PaletteTuiles(content.Load<Texture2D>("Textures\\Monde\\TilesPath"), 48, 48);
        }

        /// <summary>
        /// Fonction membre surchargeable indiquant si le sprite donné a atteint une sortie
        /// du monde. Ce monde ne comporte aucune sortie.
        /// </summary>
        /// <param name="sprite">Sprite dont on doit vérifier s'il a atteint une sortie.</param>
        /// <returns>Vrai si le sprite a atteint une sorite; faux sinon.</returns>
        public override bool AtteintUneSortie(Sprite sprite)
        {
            return false;
        }
    }
}
