//-----------------------------------------------------------------------
// <copyright file="PFEnnemi.cs" company="Marco Lavoie">
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
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Classe abstraite servant de classe de base pour celles représentant les Ennemis du jeu
    /// appliquant le pathfinding pour tenter d'attendre une destination donnée. À chaque fois
    /// que la position du sprite est modifiée, celui-ci met à jour le chemin le plus court
    /// vers une destination prédéterminer puis va tenter de suivre cette route pour atteindre
    /// la destination.
    /// La classe exploite une instance de PFGrille pour appliquer l'algorithme de pathfinding.
    /// La classe game est responsable d'allouer une instance de PFGrille au sprite pour
    /// que celui-ci puisse se déplacer.
    /// </summary>
    public abstract class PFEnnemi : Ennemi
    {
        /// <summary>
        /// Grille de noeuds exploitée par l'algorithme de pathfinding. Si cet attribut n'est pas nul alors
        /// le pathfinding est activé pour le sprite.
        /// </summary>
        private PFGrille grillePathFinding = null;

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite.
        /// </summary>
        /// <param name="x">Position en x du sprite.</param>
        /// <param name="y">Position en y du sprite.</param>
        public PFEnnemi(float x, float y)
            : base(x, y)
        {
        }

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite. Invoque l'autre constructeur.
        /// </summary>
        /// <param name="position">Position du sprite.</param>
        public PFEnnemi(Vector2 position)
            : this(position.X, position.Y)
        {
        }

        /// <summary>
        /// Propriété (setter surchargé) retournant ou changeant la position du sprite. Le setter 
        /// est surchargé afin de mettre à jour l'algorithme de pathfinding.
        /// </summary>
        /// <value>Position du sprite.</value>
        public override Vector2 Position
        {
            // Le setter met à jour le noeud de départ de l'algorithme de pathfinding.
            set
            {
                base.Position = value;

                // Si le pathfinding est actif, mettre à jour le noeud de départ.
                if (this.grillePathFinding != null)
                {
                    this.grillePathFinding.Depart = this.Position;
                }
            }
        }

        /// <summary>
        /// Propriété (accesseur pour grillePathFinding) retournant ou changeant la grille de noeuds
        /// exploitée par l'algorithme de pathfinding. 
        /// </summary>
        /// <value>Grille de noeuds pour le pathfinding.</value>
        public PFGrille GrillePathFinding
        {
            get 
            {
                return this.grillePathFinding; 
            }

            // Le setter initialise le noeud de départ de la grille en fonction de la position courante
            // du sprite. Si la grille a déjà un noeud de destination spécifié, le pathfinding sera
            // alors automatiquement actif pour le sprite.
            set 
            {
                this.grillePathFinding = value;

                if (this.grillePathFinding != null)
                {
                    this.grillePathFinding.Depart = this.Position;
                }
            }
        }

        /// <summary>
        /// Lire de  l'input les vitesses de déplacement directionnels. Nous utilisons l'intelligence
        /// artificielle pour contrôler les mouvements de l'ogre.
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
            // Distance (en pixels) en déça de laquelle le pathfiding n'est pas appliqué,
            // considérant que le sprite est assez pr`s de sa destination. On évite ainsi
            // que le sprite "danse" autour de sa destination.
            const int Tolerance = 5;

            // Par défaut, aucune vitesse appliquée.
            vitesseNord = 0.0f;
            vitesseSud = 0.0f;
            vitesseEst = 0.0f;
            vitesseOuest = 0.0f;

            // Si on a pas ce qu'il faut pour faire du pathfinding, alors arrêter ici.
            if (this.grillePathFinding == null || this.grillePathFinding.Chemin.Count == 0)
            {
                // Au minimum, faire tourner le sprite en direction de la destination s'il y en a une disponible.
                // On exploite un try-catch au cas où aucun noeud de destination n'est disponible.
                try
                {
                    this.SeTournerVers(this.grillePathFinding.Destination);
                }
                catch (NullReferenceException)
                {
                }

                return false;
            }

            // Obtenir les coordonnées du point central de la prochaine tuile sur le chemin le
            // plus court menant à sa destination.
            Vector2 destPos = this.grillePathFinding.Chemin[0];

            // Ne pas se déplacer si le sprite est déjà assez près de sa destination.
            if (Math.Abs(destPos.X - this.Position.X) > Tolerance)
            {
                if (destPos.X < this.Position.X)
                {
                    vitesseOuest = 0.5f;
                }
                else if (destPos.X > this.Position.X)
                {
                    vitesseEst = 0.5f;
                }
            }

            if (Math.Abs(destPos.Y - this.Position.Y) > Tolerance)
            {
                if (destPos.Y < this.Position.Y)
                {
                    vitesseNord = 0.5f;
                }
                else if (destPos.Y > this.Position.Y)
                {
                    vitesseSud = 0.5f;
                }
            }

            return true;
        }
    }
}
