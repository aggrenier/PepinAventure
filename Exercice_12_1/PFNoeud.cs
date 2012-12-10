//-----------------------------------------------------------------------
// <copyright file="PFNoeud.cs" company="Marco Lavoie">
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
    using System.Linq;
    using System.Text;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Classe représentant un noeud dans la grille de noeuds (classe PFGrille) associée au pathfinding.
    /// Un noeud correspond habituellement à une tuile de la mappe du monde (non pas une tuile de la
    /// palette, mais une tuile dans la mappe monde) et est exploitée par l'algorithme de pathfinding
    /// pour déterminer si le sprite peut se déplacer sur cette tuile. Voir la classe PFGrille pour
    /// plus de détails.
    /// </summary>
    public class PFNoeud
    {
        /// <summary>
        /// Étiquette de pathfinding donnant une estimation de la distance du noeud
        /// jusqu'au noeud de destination. Cette distance est habituellement celle de
        /// Manhattan (i.e. à vol d'oiseau en ne considérant que les déplacements 
        /// horizontaux et verticaux. Voir la classe PFGrille pour les détails.
        /// </summary>
        private int h;

        /// <summary>
        /// Étiquette de pathfinding donnant la distance du noeud au noeud de départ.
        /// Cette distance est calculée par l'algorithme de pathfinding. Voir la classe 
        /// PFGrille pour les détails.
        /// </summary>
        private int g;

        /// <summary>
        /// Chaque noeud de la grille (voir classe PFGrille) correspondant habituellement
        /// à une tuile de la mappe monde, cet attribut donne la rangée de cette tuile dans
        /// la matrice de tuiles de la mappe monde. Ce paramètre est requis par 
        /// l'algorithme de pathfinding pour indiquer au sprite où se déplacer sur la 
        /// mappe monde.
        /// </summary>
        private int rangee;

        /// <summary>
        /// Chaque noeud de la grille (voir classe PFGrille) correspondant habituellement
        /// à une tuile de la mappe monde, cet attribut donne la colonne de cette tuile dans
        /// la matrice de tuiles de la mappe monde. Ce paramètre est requis par 
        /// l'algorithme de pathfinding pour indiquer au sprite où se déplacer sur la 
        /// mappe monde.
        /// </summary>
        private int colonne;

        /// <summary>
        /// Lorsque l'algorithme de pathfinding (voir classe PFGrille) étiquette un noeud
        /// de la grille, il le fait toujours à partir d'un autre noeud étiquetté auparavant.
        /// Ce noeud est donc considéré comme le "parent" de this. L'attribut Parent est
        /// utilisé pour retracer le chemin le plus court du noeud de destination au noeud
        /// de départ un fois que le pathfinding à atteint le noeud de destination.
        /// </summary>
        private PFNoeud parent;

        /// <summary>
        /// Indique si le sprite peut franchir la tuile correspondant au noeud. L'algorithme
        /// de pathfinding (voir classe PFGrille) exploite cet attribut pour éliminer des
        /// noeuds de la recherche du chemin le plus court.
        /// </summary>
        private bool franchissable;

        /// <summary>
        /// Contient les coordonnées (dans le monde) du pixel au centre de la tuile correspondant
        /// au noeud. Cette position est exploitée par le sprite pour orienter ses déplacements
        /// vers la tuile correspondant à un noeud.
        /// </summary>
        private Vector2 positionTuile;

        /// <summary>
        /// Constructeur paramétré initialisant les attributs de this. Les étiquettes H et G sont
        /// initialisés à 0, et la tuile correspondant au noeud est considérée comme franchissable.
        /// </summary>
        /// <param name="r">Rangée (dans le monde) de la tuile associée au noeud.</param>
        /// <param name="c">Colonne (dans le monde) de la tuile associée au noeud.</param>
        /// <param name="pos">Coordonnées (dans le monde) du pixel au centre de la tuile associée au noeud.</param>
        public PFNoeud(int r, int c, Vector2 pos)
        {
            this.h = this.g = 0;
            this.parent = null;

            this.rangee = r;
            this.colonne = c;
            this.positionTuile = pos;
            this.franchissable = true;
        }

        /// <summary>
        /// Constructeur paramétré initialisant les attributs de this. Les étiquettes H et G sont
        /// initialisés à 0, et la tuile correspondant au noeud est considérée comme franchissable.
        /// </summary>
        /// <param name="r">Rangée (dans le monde) de la tuile associée au noeud.</param>
        /// <param name="c">Colonne (dans le monde) de la tuile associée au noeud.</param>
        /// <param name="pos">Coordonnées (dans le monde) du pixel au centre de la tuile associée au noeud.</param>
        /// <param name="franch">Indique si le noeud est franchissable ou pas.</param>
        public PFNoeud(int r, int c, Vector2 pos, bool franch)
            : this(r, c, pos)
        {
            this.franchissable = franch;
        }

        /// <summary>
        /// Propriété (accesseur pour positionTuile) retournant ou changeant les coordonnées
        /// du pixel au centre de la tuile correspondant au noeud.
        /// </summary>
        /// <value>Coordonnées (dans le monde) du pixel au centre de la tuile associée au noeud.</value>
        public Vector2 PositionTuile
        {
            get { return this.positionTuile; }
            set { this.positionTuile = value; }
        }

        /// <summary>
        /// Propriété (accesseur pour rangee) retournant ou changeant la rangée de la position de
        /// la tuile associée au noeud dans la mappe monde.
        /// </summary>
        /// <value>Rangée de la tuile (dans la mappe monde) associée au noeud.</value>
        public int Rangee
        {
            get { return this.rangee; }
            set { this.rangee = value; }
        }

        /// <summary>
        /// Propriété (accesseur pour colonne) retournant ou changeant la colonne de la position de
        /// la tuile associée au noeud dans la mappe monde.
        /// </summary>
        /// <value>Colonne de la tuile (dans la mappe monde) associée au noeud.</value>
        public int Colonne
        {
            get { return this.colonne; }
            set { this.colonne = value; }
        }

        /// <summary>
        /// Propriété (accesseur pour h) retournant ou changeant l'étiquette H du noeud.
        /// Voir l'attribut h pour plus de détails.
        /// </summary>
        /// <value>Étiquette H du noeud (distance Manhattan à la destination).</value>
        public int H
        {
            get { return this.h; }
            set { this.h = value; }
        }

        /// <summary>
        /// Propriété (accesseur pour g) retournant ou changeant l'étiquette G du noeud.
        /// Voir l'attribut h pour plus de détails.
        /// </summary>
        /// <value>Étiquette G du noeud (distance au noeud de départ).</value>
        public int G
        {
            get { return this.g; }
            set { this.g = value; }
        }

        /// <summary>
        /// Propriété (accesseur pour parent) retournant ou changeant le noeud parent du 
        /// noeud this. Voir l'attribut parent pour plus de détails.
        /// </summary>
        /// <value>Noeud parent de this, selon l'algorithme de pathfinding.</value>
        public PFNoeud Parent
        {
            get { return this.parent; }
            set { this.parent = value; }
        }

        /// <summary>
        /// Propriété (accesseur pour franchissable) retournant ou changeant l'indicateur
        /// d'opacité de la tuile associée au noeud. Cet attribut indique si le sprite peut
        /// franchir ou non la tuile lors de ses déplacements.
        /// </summary>
        /// <value>Indique si la tuile associée au noeud est franchissable.</value>
        public bool Franchissable
        {
            get { return this.franchissable; }
            set { this.franchissable = value; }
        }

        /// <summary>
        /// Propriété retournant ou changeant l'étiquette F du noeud. Cette étiquette est
        /// calculée (F = G + H) et exploitée par l'algorithme de pathfinding pour classer
        /// les noeuds dans la liste ouverte (voir classe PFGrille pour plus de détails).
        /// </summary>
        /// <value>Étiquette F de this, selon l'algorithme de pathfinding.</value>
        public int F
        {
            get { return this.G + this.H; }
        }
    }
}
