//-----------------------------------------------------------------------
// <copyright file="Ennemi.cs" company="Marco Lavoie">
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
    /// Classe abstraite servant de classe de base pour celles représentant les Ennemis du jeu. 
    /// </summary>
    public abstract class Ennemi : Personnage
    {
        /// <summary>
        /// Constructeur paramétré recevant la position du sprite.
        /// </summary>
        /// <param name="x">Position en x du sprite.</param>
        /// <param name="y">Position en y du sprite.</param>
        public Ennemi(float x, float y)
            : base(x, y)
        {
        }

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite. Invoque l'autre constructeur.
        /// </summary>
        /// <param name="position">Position du sprite.</param>
        public Ennemi(Vector2 position)
            : this(position.X, position.Y)
        {
        }

        /// <summary>
        /// Lire de  l'input les vitesses de déplacement directionnels. Un ennemi est par défaut
        /// stationnaire et inactif.
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
            // Par défaut, aucune vitesse appliquée.
            vitesseNord = 0.0f;
            vitesseSud = 0.0f;
            vitesseEst = 0.0f;
            vitesseOuest = 0.0f;

            return true;
        }
    }
}
