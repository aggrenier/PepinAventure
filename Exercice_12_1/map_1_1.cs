//-----------------------------------------------------------------------
// <copyright file="Map_1_1.cs" company="Marco Lavoie">
// Marco Lavoie, 2010. Tous droits r�serv�s
// 
// L'utilisation de ce mat�riel p�dagogique (pr�sentations, code source 
// et autres) avec ou sans modifications, est permise en autant que les 
// conditions suivantes soient respect�es:
//
// 1. La diffusion du mat�riel doit se limiter � un intranet dont l'acc�s
//    est imit� aux �tudiants inscrits � un cours exploitant le dit 
//    mat�riel. IL EST STRICTEMENT INTERDIT DE DIFFUSER CE MAT�RIEL 
//    LIBREMENT SUR INTERNET.
// 2. La redistribution des pr�sentations contenues dans le mat�riel 
//    p�dagogique est autoris�e uniquement en format Acrobat PDF et sous
//    restrictions stipul�es � la condition #1. Le code source contenu 
//    dans le mat�riel p�dagogique peut cependant �tre redistribu� sous 
//    sa forme  originale, en autant que la condition #1 soit �galement 
//    respect�e.
// 3. Le mat�riel diffus� doit contenir int�gralement la mention de 
//    droits d'auteurs ci-dessus, la notice pr�sente ainsi que la
//    d�charge ci-dessous.
// 
// CE MAT�RIEL P�DAGOGIQUE EST DISTRIBU� "TEL QUEL" PAR L'AUTEUR, SANS 
// AUCUNE GARANTIE EXPLICITE OU IMPLICITE. L'AUTEUR NE PEUT EN AUCUNE 
// CIRCONSTANCE �TRE TENU RESPONSABLE DE DOMMAGES DIRECTS, INDIRECTS, 
// CIRCONSTENTIELS OU EXEMPLAIRES. TOUTE VIOLATION DE DROITS D'AUTEUR 
// OCCASIONN� PAR L'UTILISATION DE CE MAT�RIEL P�DAGOGIQUE EST PRIS EN 
// CHARGE PAR L'UTILISATEUR DU DIT MAT�RIEL.
// 
// En utilisant ce mat�riel p�dagogique, vous acceptez implicitement les
// conditions et la d�charge exprim�s ci-dessus.
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
    /// Classe concr�tisant un monde d'image de Mario Bros.
    /// </summary>
    public class Map_1_1 : MondeImages
    {
        /// <summary>
        /// Attribut fournissant les textures d'affichage � la propri�t� Textures.
        /// </summary>
        private static Texture2D[,] textures;

        /// <summary>
        /// Attribut fournissant les textures de d�tection de collisions � la 
        /// propri�t� TexturesCollisions.
        /// </summary>
        private static Texture2D[,] texturesCollisions;

        /// <summary>
        /// Surcharge de la propri�t� accesseur aux textures d'affichage. Cette
        /// propri�t� est exploit�e par la classe de base pour afficher le
        /// monde d'images.
        /// </summary>
        public override Texture2D[,] Textures
        {
            get { return textures; }
        }

        /// <summary>
        /// Surcharge de la propri�t� accesseur aux textures de d�tection de
        /// collisions. Cette propri�t� est exploit�e par la classe de base pour 
        /// extraire les couleurs de collisions.
        /// </summary>
        public override Texture2D[,] TexturesCollisions
        {
            get { return texturesCollisions; }
        }

        /// <summary>
        /// Surcharghe de la propri�t� ccesseur retournant la position initiale du sprite 
        /// du joueur dans le monde.
        /// </summary>
        public override Vector2 PositionInitiale
        {
            get { return new Vector2(300, 480); }
        }

        /// <summary>
        /// Charge les images d'affichage et de d�tection de collisions.
        /// </summary>
        /// <param name="content">Gestionnaire de contenu permettant de charger les images du vaisseau.</param>
        /// <param name="graphics">Gestionanire de p�riph�rique d'affichage permettant d'extraire
        /// les caract�ristiques de celui-ci (p.ex. l'�cran).</param>
        public static void LoadContent(ContentManager content, GraphicsDeviceManager graphics)
        {
            // Cr�er les deux tableaux de textures.
            textures = new Texture2D[2, 2];
            texturesCollisions = new Texture2D[2, 2];

            // Charger les textures d'affichage, rang�e par rang�e
            textures[0, 0] = content.Load<Texture2D>("Monde\\map_1_1\\map00");
            textures[0, 1] = content.Load<Texture2D>("Monde\\map_1_1\\map01");

            textures[1, 0] = content.Load<Texture2D>("Monde\\map_1_1\\map10");
            textures[1, 1] = content.Load<Texture2D>("Monde\\map_1_1\\map11");

            // Charger les textures de collisions, rang�e par rang�e
            texturesCollisions[0, 0] = content.Load<Texture2D>("Monde\\map_1_1\\mapCollision00");
            texturesCollisions[0, 1] = content.Load<Texture2D>("Monde\\map_1_1\\mapCollision01");

            texturesCollisions[1, 0] = content.Load<Texture2D>("Monde\\map_1_1\\mapCollision10");
            texturesCollisions[1, 1] = content.Load<Texture2D>("Monde\\map_1_1\\mapCollision11");
        }

        /// <summary>
        /// Surcharghe de la propri�t� ccesseur retournant la position initiale du sprite       
        /// </summary>
        /// <param name="sprite">Sprite qui a atteint une sortie</param>
        /// <returns>Returns true si le joueur a atteint une sortie</returns>
        public override bool AtteintUneSortie(Sprite sprite)
        {
            return sprite.Position.Y < 60
                || sprite.Position.Y > 510;
        }     
    }
}
