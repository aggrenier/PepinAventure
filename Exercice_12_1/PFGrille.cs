//-----------------------------------------------------------------------
// <copyright file="PFGrille.cs" company="Marco Lavoie">
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
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Classe exploitant une grille de PFNoeud afin d'effectuer du pathfinding pour un sprite.
    /// Chaque noeud de la grille (attribut grille) correspond généralement à une tuile dans 
    /// le monde de tuiles où peut se déplacer un sprite. Chacun de ses noeuds sert à indiquer
    /// si la tuile correspondante est franchissable ou non (i.e. si le sprite peut s'y
    /// déplacer).
    /// Pour exploiter une instance de PFGrille, il faut premièrement lui assigner un monde
    /// de tuiles (via la propriété Monde). La grille est alors automatiquement construite
    /// (voir setter de Monde) afin qu'elle représente chaque tuile du monde par un noeud
    /// (instances de PFNoeud) contenant les informations essentielles de cette tuile (p.ex.
    /// si elle est franchissable, sa position dans le monde et les étiquettes necéssaires
    /// à l'algorithme A* (G, H et F).
    /// De plus, il faut aussi assigner un point de départ (via la propriété Depart)
    /// et une destination (via la propriété Destination) à l'instance pour que celle-ci
    /// puisse appliquer le pathfinding (A*) pour identifier le chemin le plus court
    /// entre les deux noeuds correspondants à ces points, tout en ne considérant que les 
    /// noeuds franchissables.
    /// À noter que les membres de la classe sont définis de telle sorte qu'aucune instance
    /// de PFNoeud gérées par PFGrille ne soit accessible publiquement. Ceci permet d'éviter
    /// que la grille de noeuds ne soit modifiée illégalement de l'externe.
    /// </summary>
    public class PFGrille
    {
        /// <summary>
        /// Matrice d'instances de PFNoeud correspondant aux tuiles du monde. Chaque tuile
        /// contient les informations essentielles de cette tuile (p.ex. si elle est 
        /// franchissable, sa position dans le monde et les étiquettes necéssaires à 
        /// l'algorithme A* (G, H et F).
        /// Voir le setter de la propriété Monde pour plus d'information.
        /// </summary>
        private PFNoeud[,] grille;

        /// <summary>
        /// Noeud de départ considéré par l'algorithme de pathfinding A*. L'algorithme
        /// cherche le chemin le plus court entre depart et destination.
        /// </summary>
        private PFNoeud depart;

        /// <summary>
        /// Noeud de destination considéré par l'algorithme de pathfinding A*. L'algorithme
        /// cherche le chemin le plus court entre depart et destination.
        /// </summary>
        private PFNoeud destination;

        /// <summary>
        /// Monde de tuiles à partir duquel la matrice de noeuds est construite.
        /// </summary>
        private MondeTuiles monde;

        /// <summary>
        /// La liste de noeuds constituant le chemin le plus court entre depart et 
        /// destination. Cette liste est produite automatiquement via une invocation
        /// de la routine RecalculerChemin() lorsqu'une des propriétés suivantes est
        /// modifiée : Monde, Depart et/ou Destination.
        /// </summary>
        private List<PFNoeud> cheminDeNoeuds;

        /// <summary>
        /// Constructeur paramétré initialisant les attributs de this. Invoque la fonction privée
        /// Initialiser() qui fait la quasi-totalité du travail d'initialisation de l'instance.
        /// </summary>
        /// <param name="monde">Monde de tuiles à utiliser pour construire la grille.</param>
        /// <param name="couleurFranchissable">Couleur à considérer comme franchissable (voir 
        /// fonction Initialiser()).</param>
        public PFGrille(MondeTuiles monde, Color couleurFranchissable)
        {
            // Invoquer l'autre constructeur de la classe.
            List<Color> couleursFranchissables = new List<Color>();
            couleursFranchissables.Add(couleurFranchissable);

            this.Initialiser(monde, couleursFranchissables);
        }

        /// <summary>
        /// Constructeur paramétré initialisant les attributs de this. Invoque la fonction privée
        /// Initialiser() qui fait la quasi-totalité du travail d'initialisation de l'instance.
        /// </summary>
        /// <param name="monde">Monde de tuiles à utiliser pour construire la grille.</param>
        /// <param name="couleursFranchissables">Liste de couleurs à considérer comme franchissable 
        /// (voir fonction Initialiser()).</param>
        public PFGrille(MondeTuiles monde, List<Color> couleursFranchissables)
        {
            this.Initialiser(monde, couleursFranchissables);
        }

        /// <summary>
        /// Constructeur de copie. Le chemin (s'il existe) n'est cependant pas copier mais sera
        /// de toute façon automatiquement reconstruit au besoin.
        /// </summary>
        /// <param name="grille">Grille à recopier dans this.</param>
        public PFGrille(PFGrille grille)
        {
            this.Monde = grille.Monde;

            // Copier les noeuds de grille dans ceux de this.
            for (int row = 0; row < this.NombreRangees; row++)
            {
                for (int col = 0; col < this.NombreColonnes; col++)
                {
                    this.grille[row, col].Franchissable = grille[row, col].Franchissable;
                }
            }

            // Déterminer les noeuds de départ et de destination (s'ils existent).
            try
            {
                this.Depart = grille.Depart;
                this.Destination = grille.Destination;
            }
            catch (NullReferenceException)
            {
                this.depart = this.destination = null;
            }
        }

        /// <summary>
        /// Propriété de lecture retournant la hauteur de la grille de noeuds. Ceci 
        /// correspond à la première dimension de grille.
        /// </summary>
        /// <value>Nombre de rangées dans la matrice grille.</value>
        public int NombreRangees
        {
            get { return (int)this.grille.GetLongLength(0); }
        }

        /// <summary>
        /// Propriété de lecture retournant la largeur de la grille de noeuds. Ceci 
        /// correspond à la deuxième dimension de grille.
        /// </summary>
        /// <value>Nombre de colonnes dans la matrice grille.</value>
        public int NombreColonnes
        {
            get { return (int)this.grille.GetLongLength(1); }
        }

        /// <summary>
        /// Propriété (accesseur de depart) retournant ou changeant le noeud de départ
        /// considéré par l'algorithme de pathfinding. La propriété est de type Vector2 afin
        /// d'éviter de retourner directement un noeud de la matrice. Elle considère plutôt
        /// la position de la tuile associée au noeud (i.e. propriété PositionTuile de PFNoeud)
        /// pour identifier le noeud d'intérêt.
        /// </summary>
        /// <value>Position (dans le monde) du centre de la tuile correspondant au noeud de départ.
        /// Notez que le setter accepte les coordonnées de n'importe quel pixel de la tuile.</value>
        public Vector2 Depart
        {
            // Retourne la position (dans le monde) du pixel au centre de la tuile associée
            // au noeud de départ.
            get
            {
                return this.depart.PositionTuile;
            }

            // Change noeud de départ à celui correspondant à la tuile du monde localisée à la
            // position donnée.
            set
            {
                PFNoeud oldDepart = this.depart;    // afin de déterminer si le noeud a changé

                // Calculer la position de la tuile du monde contenant le point donné.
                int row = (int)(value.Y / this.Monde.PaletteCollisions.HauteurTuile);
                int col = (int)(value.X / this.Monde.PaletteCollisions.LargeurTuile);

                // Récupérer le noeud correspondant à cette tuile.
                try
                {
                    this.depart = this.grille[row, col];
                }
                catch (IndexOutOfRangeException)
                {
                    this.depart = null;
                }

                // Si le noeud de départ fut modifié, il faut probablement recalculer le chemin 
                // le plus court vers la destination.
                if (oldDepart != this.depart)
                {
                    // Avant de complètement éliminer le chemin actuel, vérifier si la nouvelle
                    // destination est sur le chemin. Si c'est le cas alors mettre à jour ce dernier
                    // en y retirant les noeuds précédent celui de départ dans le chemin.
                    if (this.cheminDeNoeuds != null)
                    {
                        // Trouver l'index du noeud de départ dans le chemin.
                        int idx = this.cheminDeNoeuds.FindIndex(delegate(PFNoeud n) { return n == this.depart; });

                        // Si on a trouvé le noeud de départ dans le chemin actuel, alors on retire les
                        // noeuds précédents du chemin (incluant le noeud de départ).
                        if (idx >= 0)
                        {
                            this.cheminDeNoeuds.RemoveRange(0, idx + 1);
                        }
                        else
                        {
                            // Puisque le nouveau noeud de départ n'est pas sur le chemin actuel, forcer la 
                            // mise à jour du chemin le plus court (i.e. celui-ci devra être recalculé lorsque
                            // le chemin sera requis).
                            this.cheminDeNoeuds = null;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Propriété (accesseur de destination) retournant ou changeant le noeud de destination
        /// considéré par l'algorithme de pathfinding. La propriété est de type Vector2 afin
        /// d'éviter de retourner directement un noeud de la matrice. Elle considère plutôt
        /// la position de la tuile associée au noeud (i.e. propriété PositionTuile de PFNoeud)
        /// pour identifier le noeud d'intérêt.
        /// </summary>
        /// <value>Position (dans le monde) du centre de la tuile correspondant au noeud de 
        /// destination. Notez que le setter accepte les coordonnées de n'importe quel pixel de la 
        /// tuile.</value>
        public Vector2 Destination
        {
            // Retourne la position (dans le monde) du pixel au centre de la tuile associée
            // au noeud de destination.
            get
            {
                return this.destination.PositionTuile;
            }

            // Change noeud de destination à celui correspondant à la tuile du monde localisée à la
            // position donnée.
            set
            {
                PFNoeud oldDestination = this.destination;    // afin de déterminer si le noeud a changé

                // Calculer la position de la tuile du monde contenant le point donné.
                int row = (int)(value.Y / this.Monde.PaletteCollisions.HauteurTuile);
                int col = (int)(value.X / this.Monde.PaletteCollisions.LargeurTuile);

                // Récupérer le noeud correspondant à cette tuile.
                try
                {
                    this.destination = this.grille[row, col];
                }
                catch (IndexOutOfRangeException)
                {
                    this.destination = null;
                }

                // Si le noeud de départ fut modifié, il faudra probablement recalculer le chemin 
                // le plus court vers la destination (i.e. celui-ci devra être recalculé lorsque
                // le chemin sera requis).
                if (oldDestination != this.destination)
                {
                    // Avant de complètement éliminer le chemin actuel, vérifier si la nouvelle
                    // destination est sur le chemin. Si c'est le cas alors mettre à jour ce dernier
                    // en y retirant les noeuds suivant celui de destination dans le chemin.
                    if (this.cheminDeNoeuds != null)
                    {
                        // Trouver l'index du noeud de destination dans le chemin (on recherche de la fin
                        // vers le début).
                        int idx = this.cheminDeNoeuds.FindLastIndex(delegate(PFNoeud n) { return n == this.destination; });

                        // Si on a trouvé le noeud de destination dans le chemin actuel, alors on retire les
                        // noeuds suivants du chemin (en y conservant cependant le noeud de destination).
                        if (idx >= 0)
                        {
                            this.cheminDeNoeuds.RemoveRange(idx + 1, this.cheminDeNoeuds.Count - idx - 1);
                        }
                        else
                        {
                            // Puisque le nouveau noeud de départ n'est pas sur le chemin actuel, forcer la 
                            // mise à jour du chemin le plus court (i.e. celui-ci devra être recalculé lorsque
                            // le chemin sera requis).
                            this.cheminDeNoeuds = null;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Propriété de lecture retournant une liste des coordonnées (instances de Vector2)
        /// constituant le chemin le plus court entre le noeud de départ (depart) et celui
        /// de destination (destination).
        /// </summary>
        /// <value>Liste de coordonnées constituant le chemin le plus court entre depart
        /// et destination.</value>
        public List<Vector2> Chemin
        {
            get
            {
                List<Vector2> liste = new List<Vector2>();  // liste à retourner

                // Ajouter un point (centre de la tuile) pour chaque noeud (i.e. tuile) du chemin.
                foreach (PFNoeud noeud in this.CheminDeNoeuds)
                {
                    liste.Add(noeud.PositionTuile);
                }

                return liste;
            }
        }

        /// <summary>
        /// Propriété (accesseur de monde) retournant ou changeant le monde de tuiles 
        /// associé à la grille. Le setter de la propriété reconstruit la grille de noeuds
        /// (i.e. attribut grille) en fonction du nouveau monde de noeuds fourni.
        /// </summary>
        /// <value>Monde de tuiles utilisée pour construire la grille de noeuds.</value>
        protected MondeTuiles Monde
        {
            get
            {
                return this.monde;
            }

            // Reconstruit la grille de noeud en fonction du nouveau monde fourni.
            set
            {
                // On s'assure d'avoir un monde avec sa palette de collisions.
                Debug.Assert(value != null && value.PaletteCollisions != null, "Le monde doit disposer d'une palette de collisions");

                this.monde = value;  // assigner le nouveau monde à l'attribut

                // Déterminer le nombre vertical et horizontal de tuiles constituant le monde
                // (requis car la grille est constituée d'un noeud par tuile du monde).
                int nombreRangees = 2;
                int nombreColonnes = 2;

                // Créer la grille de noeuds : pour chaque tuile du monde, créer un noeud dans la
                // grille.
                this.grille = new PFNoeud[nombreRangees, nombreColonnes];
                for (int row = 0; row < this.NombreRangees; row++)
                {
                    for (int col = 0; col < this.NombreColonnes; col++)
                    {
                        // Calculer les coordonnées (dans le monde) du point au centre de la tuile.
                        Vector2 centre = new Vector2(
                            (col * this.monde.PaletteCollisions.LargeurTuile) + (this.monde.PaletteCollisions.LargeurTuile / 2),
                            (row * this.monde.PaletteCollisions.HauteurTuile) + (this.monde.PaletteCollisions.HauteurTuile / 2));

                        // Créer un nouveau noeud pour la grille.
                        this.grille[row, col] = new PFNoeud(row, col, centre);
                    }
                }

                // Effacer les anciens noeuds de départ et destination.
                this.depart = null;
                this.destination = null;

                // Forcer la mise à jour du chemin le plus court.
                this.cheminDeNoeuds = null;
            }
        }

        /// <summary>
        /// Propriété de lecture (accesseur de cheminDeNoeuds) retournant la liste des
        /// noeuds sur le chemin le plus court entre le noeud de départ (depart) et celui
        /// de destination (destination). Au besoin (i.e. si le chmin n'a pas encore été
        /// calculé par pathfinding), le chemin est établi.
        /// </summary>
        /// <value>Liste de noeuds constituant le chemin le plus court entre depart
        /// et destination.</value>
        private List<PFNoeud> CheminDeNoeuds
        {
            get 
            {
                // Si le chemin n'a pas encore été calculé, le faire maintenant.
                if (this.cheminDeNoeuds == null)
                {
                    this.RecalculerChemin(out this.cheminDeNoeuds);
                }

                return this.cheminDeNoeuds; 
            }
        }

        /// <summary>
        /// Propriété indexée (accesseur par défaut pour grille) retournant ou changeant 
        /// un noeud de la grille.
        /// </summary>
        /// <param name="row">Rangée de la matrice contenant le noeud d'intérêt.</param>
        /// <param name="col">Colonne de la matrice contenant le noeud d'intérêt.</param>
        /// <value>PFNoeud contenu dans l'élément (row,col) de la grille de noeuds.</value>
        /// <returns>return la liste</returns>
        private PFNoeud this[int row, int col]
        {
            get { return this.grille[row, col]; }
            set { this.grille[row, col] = value; }
        }

        /// <summary>
        /// Fonction construisant la grille de noeuds à partir du monde de tuiles fourni en paramètre.
        /// La fonction crée la matrice (grille) de même taille que celle de tuiles du monde, puis
        /// un noeud pour chaque tuile. Le paramètre couleursFranchissables est exploité pour
        /// déterminer si chaque tuile est franchissable ou pas (si les couleurs retrouvées dans une tuiles
        /// sont toutes dans couleursFranchissables, alors la tuile est considérée franchissable).
        /// </summary>
        /// <param name="monde">Monde de tuiles à considérer pour construire pricGrille.</param>
        /// <param name="couleursFranchissables">Liste de couleurs à considérer pour déterminer si une tuile
        /// est franchissable ou pas.</param>
        public void Initialiser(MondeTuiles monde, List<Color> couleursFranchissables)
        {
            this.Monde = monde;     // socker l'instance de MondeDeTuiles

            // Déterminer pour chaque tuile de la palette lesquelles sont franchissables.
            bool[] franchissables = new bool[this.Monde.PaletteCollisions.NombreDeTuiles];
            for (int tuileIdx = 0; tuileIdx < franchissables.GetLength(0); tuileIdx++)
            {
                franchissables[tuileIdx] = true;

                // Obtenir la liste des couleurs retrouvées dans la tuile courante.
                List<Color> couleurs;
                this.Monde.PaletteCollisions.CouleursDansTuile(tuileIdx, out couleurs);

                // Pour chaque couleur de la tuile, vérifier si elle est dans couleursFranchissables 
                // (sinon la tuile n'est pas considérée comme franchissable).
                foreach (Color clr in couleurs)
                {
                    if (!couleursFranchissables.Exists(delegate(Color c) { return c == clr; }))
                    {
                        franchissables[tuileIdx] = false;
                        break;
                    }
                }
            }

            // Indiquer pour chaque noeud de la grille s'il est franchissable ou non.
            for (int row = 0; row < this.NombreRangees; row++)
            {
                for (int col = 0; col < this.NombreColonnes; col++)
                {
                    // Calculer les coordonnées du centre de la tuile correspondant au noeud.
                    Vector2 centre = new Vector2(
                        (col * this.Monde.PaletteCollisions.LargeurTuile) + (this.Monde.PaletteCollisions.LargeurTuile / 2),
                        (row * this.Monde.PaletteCollisions.HauteurTuile) + (this.Monde.PaletteCollisions.HauteurTuile / 2));

                    // Obtenir l'index de cette tuile dans la palette du monde.
                    int tuileIdx = this.Monde.MondeXY2TuileIdx(centre);

                    // Configurer le noeud en conséquence.
                    this.grille[row, col].Franchissable = franchissables[tuileIdx];
                }
            }
        }

        /// <summary>
        /// Fonction appliquant le pathfinding (A*) pour trouver le chemin le plus court entre
        /// le noeud de départ (depart) et le noeud de destination (destination) dans la
        /// grille de noeuds tout en ne considérant que les tuiles franchissable. Si aucun chemin 
        /// n'est trouvé, la fonction retourne faux mais retourne quand même un chemin de noeuds
        /// menant le plus près possible de la destination.
        /// Pour comprendre le code de cette fonction il faut être familier avec l'algorithme de
        /// pathfinding A*.
        /// </summary>
        /// <param name="cheminNoeuds">Liste de noeuds constituant le plus court chemin franchissable 
        /// trouvé entre depart et destination.</param>
        /// <returns>Vrai si un chemin du noeud de départ à celui de destination fut trouvé; faux 
        /// sinon (un chemin a toufois peut-être été retourné tout de même, mais si c'est le cas il
        /// ne mène pas à la destination mais tout près)</returns>
        private bool RecalculerChemin(out List<PFNoeud> cheminNoeuds)
        {
            // S'assurer que la liste fournie ne contient aucun noeud.
            cheminNoeuds = new List<PFNoeud>();

            // Premièrement s'assurer qu'on a un noeud de départ ainsi qu'un noeud de destination.
            if (this.depart == null || this.destination == null)
            {
                return false;
            }

            // Effacer les étiquettes de noeuds ayant pu être calculées précédemment.
            for (int row = 0; row < this.NombreRangees; row++)
            {
                for (int col = 0; col < this.NombreColonnes; col++)
                {
                    this.grille[row, col].G = 0;

                    this.grille[row, col].H = (Math.Abs(this.destination.Rangee - this.grille[row, col].Rangee) +
                                               Math.Abs(this.destination.Colonne - this.grille[row, col].Colonne)) * 10;

                    this.grille[row, col].Parent = null;
                }
            }

            bool destinationAtteinte = false;   // indique si la destination fut atteinte
            PFNoeud noeudCourant = null;        // dernier noeud extrait de la liste ouverte

            // Créer les listes de noeuds ouverts et fermés.
            PFListeNoeuds<PFNoeud> listeOuverte = new PFListeNoeuds<PFNoeud>();
            PFListeNoeuds<PFNoeud> listeFermee = new PFListeNoeuds<PFNoeud>();

            // Initialiser la recherche au noeud de départ.
            listeOuverte.Add(this.depart);

            // Tant qu'on a des noeuds ouverts et que la destination n'est pas atteinte,
            // poursuivre la recherche.
            while (listeOuverte.Count > 0)
            {
                // Considérer le noeud ouvert avec la plus petite valeur F et le transférer de
                // la liste des noeuds ouverts à celle des noeuds fermés.
                noeudCourant = listeOuverte[0];
                listeOuverte.RemoveAt(0);
                listeFermee.Add(noeudCourant);

                // Vérifier si on a atteint le noeud de destination. Si oui alors on sort de la boucle
                // de recherche.
                if (noeudCourant == this.destination)
                {
                    destinationAtteinte = true;
                    break;
                }

                // Obtenir la liste des noeuds voisins franchissables.
                List<PFNoeud> noeudsPossibles = this.NoeudsVoisinsFranchissables(noeudCourant);

                // Retirer des noeuds voisins potentiels ceux étant dans la liste de noeuds fermés.
                int idx = 0;
                while (idx < noeudsPossibles.Count)
                {
                    if (listeFermee.Contains(noeudsPossibles[idx]))
                    {
                        noeudsPossibles.RemoveAt(idx);
                    }
                    else
                    {
                        idx++;
                    }
                }

                // Mettre à jour les attributs des noeuds voisins pouvant être potentiellement
                // sur le chemin le plus court à la destination.
                for (int i = 0; i < noeudsPossibles.Count; i++)
                {
                    // Calculer la nouvelle valeur G du noeud : 14 + G du noeud courant si le voisin est diagonal
                    // au noeud courant, 10 + G du noeud courant autrement.
                    int valeurG;
                    if (noeudsPossibles[i].Rangee != noeudCourant.Rangee && noeudsPossibles[i].Colonne != noeudCourant.Colonne)
                    {
                        valeurG = noeudCourant.G + 14;
                    }
                    else
                    {
                        valeurG = noeudCourant.G + 10;
                    }

                    // Si le noeud n'a jamais été considéré, alors l'ajouter à la liste des noeuds
                    // ouverts; s'il est déjà dans la liste ouverte, alors le mettre à jour au besoin.
                    if (noeudsPossibles[i].Parent == null)
                    {
                        noeudsPossibles[i].G = valeurG;
                        noeudsPossibles[i].Parent = noeudCourant;

                        listeOuverte.InsertionDichotomique(noeudsPossibles[i]);
                    }
                    else if (valeurG < noeudsPossibles[i].G)
                    {
                        noeudsPossibles[i].G = valeurG;
                        noeudsPossibles[i].Parent = noeudCourant;
                    }
                }
            }

            // Il n'existe aucun chemin permettant d'atteindre le noeud de destination à
            // partir du noeud de départ. On retourne alors le chemin permettant de se
            // raprocher le plus près possible de la destination.
            if (!destinationAtteinte)
            {
                // Trouver dans la liste des noeuds fermés celui le plus près possible de la 
                // destination (distance à vol d'oiseau, i.e. avec la plus petite valeur de H).
                if (listeFermee.Count > 0)
                {
                    noeudCourant = listeFermee[0];
                    for (int i = 1; i < listeFermee.Count; i++)
                    {
                        if (listeFermee[i].H < noeudCourant.H)
                        {
                            noeudCourant = listeFermee[i];
                        }
                    }
                }
                else
                {
                    return false;
                }
            }

            // Reconstituer le chemin vers le noeud de départ. En "empilant" les noeuds sur
            // le parcours du noeud de destination vers le noeud de départ, on contitue le
            // chemin du départ à la destination.
            while (noeudCourant.Parent != null)
            {
                cheminNoeuds.Insert(0, noeudCourant);
                noeudCourant = noeudCourant.Parent;
            }
            
            return destinationAtteinte;
        }

        /// <summary>
        /// Fonction retournant une liste des noeuds franchissables adjacents à celui donné en paramètre.
        /// Les huit noeuds adjacents sont considérés:
        ///    +-----------+  
        ///    | 1 | 2 | 3 |  Les noeuds 2, 4, 5 et 7 sont validés uniquement s'ils sont franchissables.
        ///    |---+---+---|  
        ///    | 4 |   | 5 |  Les noeuds en diagonal (1, 3, 6 et 8) sont validés s'ils sont
        ///    |---+---+---|  franchissables et que leurs deux noeuds adjacents le sont aussi. Par exemple
        ///    | 6 | 7 | 8 |  le noeud 6 est valide si les noeuds 4, 6 et 7 sont franchissables.
        ///    +-----------+
        /// </summary>
        /// <param name="noeud">Noeud dont on doit déterminer les noeuds adjacents franchissables.</param>
        /// <returns>Liste des noeuds franchissables adjacents au noeud fourni en paramètre.</returns>
        private List<PFNoeud> NoeudsVoisinsFranchissables(PFNoeud noeud)
        {
            // Liste où stocker les voisins franchissables.
            List<PFNoeud> liste = new List<PFNoeud>();

            // Valider le noeud adjacent #2.
            if (noeud.Rangee > 0)
            {
                if (this.grille[noeud.Rangee - 1, noeud.Colonne].Franchissable)
                {
                    // Noeud adjacent #2 valide.
                    liste.Add(this.grille[noeud.Rangee - 1, noeud.Colonne]);
                }
            }

            // Valider le noeud adjacent #7.
            if (noeud.Rangee < this.NombreRangees - 1)
            {
                if (this.grille[noeud.Rangee + 1, noeud.Colonne].Franchissable)
                {
                    // Noeud adjacent #7 valide.
                    liste.Add(this.grille[noeud.Rangee + 1, noeud.Colonne]);
                }
            }

            // Valider le noeud adjacent #4.
            if (noeud.Colonne > 0)
            {
                if (this.grille[noeud.Rangee, noeud.Colonne - 1].Franchissable)
                {
                    // Noeud adjacent #4 valide.
                    liste.Add(this.grille[noeud.Rangee, noeud.Colonne - 1]);
                }
            }

            // Valider le noeud adjacent #5.
            if (noeud.Colonne < this.NombreColonnes - 1)
            {
                if (this.grille[noeud.Rangee, noeud.Colonne + 1].Franchissable)
                {
                    // Noeud adjacent #5 valide.
                    liste.Add(this.grille[noeud.Rangee, noeud.Colonne + 1]);
                }
            }

            // Valider le noeud adjacent #1.
            if (noeud.Rangee > 0 && noeud.Colonne > 0)
            {
                if (this.grille[noeud.Rangee - 1, noeud.Colonne].Franchissable &&
                    this.grille[noeud.Rangee, noeud.Colonne - 1].Franchissable &&
                    this.grille[noeud.Rangee - 1, noeud.Colonne - 1].Franchissable)
                {
                    // Noeud adjacent #1 valide.
                    liste.Add(this.grille[noeud.Rangee - 1, noeud.Colonne - 1]);
                }
            }

            // Valider le noeud adjacent #3.
            if (noeud.Rangee > 0 && noeud.Colonne < this.NombreColonnes - 1)
            {
                if (this.grille[noeud.Rangee - 1, noeud.Colonne].Franchissable &&
                    this.grille[noeud.Rangee, noeud.Colonne + 1].Franchissable &&
                    this.grille[noeud.Rangee - 1, noeud.Colonne + 1].Franchissable)
                {
                    // Noeud adjacent #3 valide.
                    liste.Add(this.grille[noeud.Rangee - 1, noeud.Colonne + 1]);
                }
            }

            // Valider le noeud adjacent #6.
            if (noeud.Rangee < this.NombreRangees - 1 && noeud.Colonne > 0)
            {
                if (this.grille[noeud.Rangee + 1, noeud.Colonne].Franchissable &&
                    this.grille[noeud.Rangee, noeud.Colonne - 1].Franchissable &&
                    this.grille[noeud.Rangee + 1, noeud.Colonne - 1].Franchissable)
                {
                    // Noeud adjacent #6 valide.
                    liste.Add(this.grille[noeud.Rangee + 1, noeud.Colonne - 1]);
                }
            }

            // Valider le noeud adjacent #8.
            if (noeud.Rangee < this.NombreRangees - 1 && noeud.Colonne < this.NombreColonnes - 1)
            {
                if (this.grille[noeud.Rangee + 1, noeud.Colonne].Franchissable &&
                    this.grille[noeud.Rangee, noeud.Colonne + 1].Franchissable &&
                    this.grille[noeud.Rangee + 1, noeud.Colonne + 1].Franchissable)
                {
                    // Noeud adjacent #8 valide.
                    liste.Add(this.grille[noeud.Rangee + 1, noeud.Colonne + 1]);
                }
            }

            return liste;
        }
    }
}
