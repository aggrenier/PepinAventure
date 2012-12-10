//-----------------------------------------------------------------------
// <copyright file="PFListeNoeuds.cs" company="Marco Lavoie">
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

    /// <summary>
    /// Classe implantant une liste d'instances de PFNoeud afin de permettre l'insertion dichotomique
    /// selon l'étiquette F des noeuds. Cette classe surcharge la classe List afin d'offrir la nouvelle
    /// fonction d'insertion. En exploitant celle-ci pour insérer des noeuds dans la liste, aucune 
    /// invocation de sort() est requise et le noeud avec la plus petite valeur de F est toujours en
    /// début de liste. L'algorithme de pathfinding (voir la classe PFGrille) exploite une telle liste
    /// pour gérer les noeuds ouverts (i.e. noeuds étiquettés mais pour lesquels la distance exacte
    /// à la destination n'est pas encore déterminée). 
    /// </summary>
    /// <typeparam name="T">Doit être de type PFNoeud ou descendant.</typeparam>
    public class PFListeNoeuds<T> : List<T> where T : PFNoeud 
    {
        /// <summary>
        /// Insertion dichotomique dans la liste en fonction de la valeur F du noeud. En exploitant
        /// uniquement cette fonction pour insérer des noeuds dans la liste, les noeuds sont conservés
        /// en ordre croissant de valeurs F. On a pas ainsi à trier la liste ou à faire une recherche
        /// linéaire pour trouver le noeud avec le plus petit F.
        /// </summary>
        /// <param name="noeud">Noeud à insérer dans la liste.</param>
        public void InsertionDichotomique(T noeud)
        {
            int gauche = 0;
            int droite = this.Count - 1;
            int centre = 0;

            // Effectuer une recherche binaire pour trouver la position où insérer le noeud dans
            // la liste afin de conserver l'ordre croissant de valeurs F.
            while (gauche <= droite)
            {
                centre = (gauche + droite) / 2;
                if (noeud.F < this[centre].F)
                {
                    droite = centre - 1;
                }
                else if (noeud.F > this[centre].F)
                {
                    gauche = centre + 1;
                }
                else
                {
                    gauche = centre;
                    break;
                }
            }

            this.Insert(gauche, noeud);
        }
    }
}
