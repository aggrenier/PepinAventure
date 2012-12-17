//-----------------------------------------------------------------------
// <copyright file="Game.cs" company="Marco Lavoie">
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
    using System.IO;
    using System.Linq;

    using IFM20884;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.GamerServices;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;
    using Microsoft.Xna.Framework.Net;
    using Microsoft.Xna.Framework.Storage;

    /// <summary>
    /// Classe principale du jeu.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// Effet sonore contenant le bruitage de fond du jeu.
        /// </summary>
        private static SoundEffect bruitageFond;

        /// <summary>
        /// Effet sonore contenant le bruitage de fin du jeu.
        /// </summary>
        private static SoundEffect bruitageFin;

        /// <summary>
        /// Effet sonore contenant le bruitage fin
        /// </summary>
        private bool bruitageFinOn = false;

        /// <summary>
        /// Liste de tous les menus du jeu (chargés dans LoadContent()).
        /// </summary>
        private List<Menu> listeMenus = new List<Menu>();

        /// <summary>
        /// Menu présentement affiché.
        /// </summary>
        private Menu menuCourant = null;

        /// <summary>
        /// Police exploitée pour afficher le titre des menus.
        /// </summary>
        private SpriteFont policeMenuTitre;

        /// <summary>
        /// Police exploitée pour afficher les items de menus.
        /// </summary>
        private SpriteFont policeMenuItem;

        /// <summary>
        /// Couleur de la police exploitée pour afficher le titre des menus.
        /// </summary>
        private Color couleurMenuTitre = Color.White;

        /// <summary>
        /// Couleur de la police exploitée pour afficher les items des menus lorsqu'ils ne sont 
        /// pas actifs.
        /// </summary>
        private Color couleurMenuItem = Color.White;

        /// <summary>
        /// Couleur de la police exploitée pour afficher les items des menus lorsqu'ils sont 
        /// actifs.
        /// </summary>
        private Color couleurMenuItemSelectionne = Color.Yellow;

        /// <summary>
        /// Instance de bruitage de fond en cours de sonorisation durant le jeu.
        /// </summary>
        private SoundEffectInstance bruitageFondActif;

        /// <summary>
        /// Instance de bruitage des blocs.
        /// </summary>
        private SoundEffect bruitageblock;

        /// <summary>
        /// Réprésente la bloc en contact avec le joueur.
        /// </summary>
        private Bloc maBloc;

        /// <summary>
        /// Réprésente la durée que le joueur est en contact avec maBloc.
        /// </summary>
        private int contPousseBloc = 0;

        /// <summary>
        /// Conteur pour l'animation de mourir.
        /// </summary>
        private int contMort = 0;

        /// <summary>
        /// Instance de bruitage de fond en cours de sonorisation durant le jeu.
        /// </summary>
        private SoundEffectInstance bruitageGameOverActif;

        /// <summary>
        /// Instance de bruitage des blocs.
        /// </summary>
        private SoundEffect bruitageGameOver;

        /// <summary>
        /// Instance de bruitage de mort du joueur.
        /// </summary>
        private SoundEffectInstance bruitageMortActif;

        /// <summary>
        /// Instance de bruitage de mort du joueur.
        /// </summary>
        private SoundEffect bruitageMort;

        /// <summary>
        /// Instance de bruitage de mort du joueur.
        /// </summary>
        private SoundEffect bruitageFrapper;

        /// <summary>
        /// Instance de bruitage quand le joueur prends des items.
        /// </summary>
        private SoundEffect bruitageItem;

        /// <summary>
        /// Fond d'écran d'accueil.
        /// </summary>
        private Texture2D ecranAccueil;

        /// <summary>
        /// Fond d'écran de générique.
        /// </summary>
        private Texture2D ecranGenerique;

        /// <summary>
        /// Fond d'écran de générique.
        /// </summary>
        private Texture2D ecranGameOver;

        /// <summary>
        /// Donne la position d'affichage du générique lorsque le jeu est en cours
        /// de se terminer.
        /// </summary>
        private int generiqueScrollPos;

        /// <summary>
        /// Attribut permettant d'obtenir des infos sur la carte graphique et l'écran.
        /// </summary>
        private GraphicsDeviceManager graphics;

        /// <summary>
        /// Attribut gérant l'affichage en batch à l'écran.
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// Liste des sprites représentant des projectiles.
        /// </summary>
        private List<Sprite> listeProjectiles;

        /// <summary>
        /// Liste des sprites représentant des projectiles fini.
        /// </summary>
        private List<Sprite> listeProjectileFini;

        /// <summary>
        /// Liste des sprites représentant des blocs.
        /// </summary>
        private List<Sprite> listeBloc;

        /// <summary>
        /// Liste des sprites représentant des blocs fini.
        /// </summary>
        private List<Sprite> listeBlocFini;

        /// <summary>
        /// Liste des sprites représentant des switch.
        /// </summary>
        private List<Sprite> listeSwitch;

        /// <summary>
        /// Liste des sprites représentant des switch fini.
        /// </summary>
        private List<Sprite> listeSwitchFini;

        /// <summary>
        /// Liste des portes animés.
        /// </summary>
        private List<Sprite> listePorte;

        /// <summary>
        /// Liste des portes animés.
        /// </summary>
        private List<Sprite> listePorteHorizontale;

        /// <summary>
        /// Liste des portes animés.
        /// </summary>
        private List<Sprite> listePorteHorizontaleFini;

        /// <summary>
        /// Sert 'a enlever les portes des maps suivants.
        /// </summary>
        private List<Sprite> listePorteFini;

        /// <summary>
        /// Liste des portes animés.
        /// </summary>
        private List<Sprite> listeVideJoueur;

        /// <summary>
        /// Sert 'a enlever les portes des maps suivants.
        /// </summary>
        private List<Sprite> listeVideJoueurFini;

        /// <summary>
        /// Liste des portes animés.
        /// </summary>
        private List<Sprite> listeFood;

        /// <summary>
        /// Sert 'a enlever les portes des maps suivants.
        /// </summary>
        private List<Sprite> listeFoodFini;

        /// <summary>
        /// Liste des portes animés.
        /// </summary>
        private List<Sprite> listeClef;

        /// <summary>
        /// Sert 'a enlever les portes des maps suivants.
        /// </summary>
        private List<Sprite> listeClefFini;

        /// <summary>
        /// Sert 'a enlever les portes des maps suivants.
        /// </summary>
        private bool boolFood;

        /// <summary>
        /// Sert à ouvrir les portesClef des maps suivants.
        /// </summary>
        private bool boolClef;

        /// <summary>
        /// Tableau de porteClef ouvert. Cela réside dans Game à cause qu'il faut être sauvegarder en tout temps.
        /// </summary>
        private bool[] portesClefOuvert = new bool[1];

        /// <summary>
        /// Attribut représentant le monde de tuiles à afficherdurant le jeu.
        /// </summary>
        private Monde monde;

        /// <summary>
        /// Attribut représentant le personnage contrôlé par le joueur.
        /// </summary>
        private Personnage joueur;

        /// <summary>
        /// Liste de sprite représentant les ogres.
        /// </summary>
        private List<Ennemi> listeOgres;

        /// <summary>
        /// Liste de sprite représentant les ogres fini.
        /// </summary>
        private List<Ennemi> listeOgresFini;

        /// <summary>
        /// Générateur de nombres aléatoires pour générer des astéroïdes.
        /// </summary>
        private Random randomPJEnemi;

        /// <summary>
        /// Probabilité de générer un astéroïde par cycle de Update().
        /// </summary>
        private float probPJ;

        /// <summary>
        /// Liste de gestion des particules d'explosions.
        /// </summary>
        private List<ParticuleExplosion> listeParticulesExplosions = new List<ParticuleExplosion>();

        /// <summary>
        /// Texture représentant une particule d'explosion.
        /// </summary>
        private Texture2D explosionParticule;

        /// <summary>
        /// Générateur de nombres aléatoires pour générer des particules d'explosion.
        /// </summary>
        private Random randomExplosions;

        /// <summary>
        /// Attribut indiquant l'état du jeu
        /// </summary>
        private Etats etatJeu;

        /// <summary>
        /// Etat dans lequel état le jeu avant que la dernière pause ne soit activée.
        /// </summary>
        private Etats prevEtatJeu;

        /// <summary>
        /// Attribut fournissant la police d'affichage pour les messages
        /// </summary>
        private SpriteFont policeMessages;

        /// <summary>
        /// Attribut représentant la camera.
        /// </summary>
        private Camera camera;

        /// <summary>
        /// Constructeur par défaut de la classe. Cette classe est générée automatiquement
        /// par Visual Studio lors de la création du projet.
        /// </summary>
        public Game()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferHeight = 600;
            this.graphics.PreferredBackBufferWidth = 600;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Enumération des mondes disponibles.
        /// </summary>
        public enum Mondes
        {
            /// <summary>
            /// Monde 1.1.
            /// </summary>
            MAP_1_1,

            /// <summary>
            /// Monde 1.2.
            /// </summary>
            MAP_1_2,

            /// <summary>
            /// Monde 1.3. 
            /// </summary>
            MAP_1_3,

            /// <summary>
            /// Monde 1.4.
            /// </summary>
            MAP_1_4,

            /// <summary>
            /// Monde 1.4.
            /// </summary>
            MAP_1_5
        }

        /// <summary>
        /// États disponibles du personnage.
        /// </summary>
        public enum Etats
        {
            /// <summary>
            /// En cours de démarrage.
            /// </summary>
            Demarrer,

            /// <summary>
            /// En cours de jeu.
            /// </summary>
            Jouer,

            /// <summary>
            /// En cours de fin de jeu.
            /// </summary>
            Quitter,

            /// <summary>
            /// En cours de fin de jeu.
            /// </summary>
            GameOver,

            /// <summary>
            /// En suspension temporaire.
            /// </summary>
            Pause
        }

        /// <summary>
        /// Proptiété gérant le monde courant. Le mutateur réinitialise les sprites
        /// et autres attributs du jeu en fonction du nouveau monde.
        /// </summary>
        public Mondes MondeCourant
        {
            get
            {
                // Retourner l'enum correspondant au monde courant.
                if (this.monde is Map_1_1)
                {
                    return Mondes.MAP_1_1;
                }
                else if (this.monde is Map_1_2)
                {
                    return Mondes.MAP_1_2;
                }
                else if (this.monde is Map_1_3)
                {
                    return Mondes.MAP_1_3;
                }
                else if (this.monde is Map_1_4)
                {
                    return Mondes.MAP_1_4;
                }
                else
                {
                    return Mondes.MAP_1_5;
                }
            }

            // Changer le monde et réinitialiser les composants du jeu en conséquence.
            set
            {
                // Créer le nouveau monde.
                if (value == Mondes.MAP_1_1)
                {
                    this.monde = new Map_1_1();
                }
                else if (value == Mondes.MAP_1_2)
                {
                    this.monde = new Map_1_2();
                }
                else if (value == Mondes.MAP_1_3)
                {
                    this.monde = new Map_1_3();
                }
                else if (value == Mondes.MAP_1_4)
                {
                    this.monde = new Map_1_4();
                }
                else if (value == Mondes.MAP_1_5)
                {
                    this.monde = new Map_1_5();
                }

                // Reconfigurer la caméra.
                if (this.camera != null)
                {
                    this.camera.MondeRect = new Rectangle(0, 0, this.monde.Largeur, this.monde.Hauteur);
                }

                // Reconfigurer le sprite du joueur.
                if (this.joueur != null)
                {
                    this.joueur.BoundsRect = new Rectangle(0, 0, this.monde.Largeur, this.monde.Hauteur);
                    this.joueur.Position = this.monde.PositionInitiale;
                }
            }
        }

        /// <summary>
        /// Propriété (accesseur pour etatJeu) retournant ou changeant l'état du jeu.
        /// </summary>
        /// <value>État courant du jeu.</value>
        public Etats EtatJeu
        {
            get { return this.etatJeu; }
            set { this.etatJeu = value; }
        }

        /// <summary>
        /// Propriété activant et désactivant l'état de pause du jeu. Cette propriété doit être utilisée
        /// pour mettre le jeu en pause (plutôt que EtatJeu) car elle stocke l'état précédent (i.e. avant 
        /// la pause) du jeu afin de le restaurer lorsque la pause est terminée.
        /// </summary>
        /// <value>Le jeu est en pause ou pas.</value>
        public bool Pause
        {
            get
            {
                return this.etatJeu == Etats.Pause;
            }

            set
            {
                // S'assurer qu'il y a changement de statut de pause
                if (value && this.EtatJeu != Etats.Pause)
                {
                    // Stocker l'état courant du jeu avant d'activer la pause
                    this.prevEtatJeu = this.EtatJeu;
                    this.EtatJeu = Etats.Pause;
                }
                else if (!value && this.EtatJeu == Etats.Pause)
                {
                    // Restaurer l'état du jeu à ce qu'il était avant la pause
                    this.EtatJeu = this.prevEtatJeu;
                }

                // Suspendre les effets sonores au besoin
                this.SuspendreEffetsSonores(this.Pause);
            }
        }

        /// <summary>
        /// Propriété activant et désactivant l'état de pause du jeu. Cette propriété doit être utilisée
        /// pour mettre le jeu en pause (plutôt que EtatJeu) car elle stocke l'état précédent (i.e. avant 
        /// la pause) du jeu afin de le restaurer lorsque la pause est terminée.
        /// </summary>
        /// <value>Le jeu est en pause ou pas.</value>
        public bool GameOverState
        {
            get
            {
                return this.etatJeu == Etats.GameOver;
            }

            set
            {
                // S'assurer qu'il y a changement de statut de pause
                if (value && this.EtatJeu != Etats.GameOver)
                {
                    // Stocker l'état courant du jeu avant d'activer la pause
                    this.prevEtatJeu = this.EtatJeu;
                    this.EtatJeu = Etats.GameOver;
                }

                // Suspendre les effets sonores au besoin
                this.SuspendreEffetsSonores(this.GameOverState);
            }
        }

        /// <summary>
        /// Propriété activant et désactivant l'état de pause du jeu. Cette propriété doit être utilisée
        /// pour mettre le jeu en pause (plutôt que EtatJeu) car elle stocke l'état précédent (i.e. avant 
        /// la pause) du jeu afin de le restaurer lorsque la pause est terminée.
        /// </summary>
        /// <value>Le jeu est en pause ou pas.</value>
        public bool Quitter
        {
            get
            {
                return this.etatJeu == Etats.Quitter;
            }

            set
            {
                // S'assurer qu'il y a changement de statut de pause
                if (value && this.EtatJeu != Etats.Quitter)
                {
                    // Stocker l'état courant du jeu avant d'activer la pause
                    this.prevEtatJeu = this.EtatJeu;
                    this.EtatJeu = Etats.Quitter;
                }
                else if (!value && this.EtatJeu == Etats.Quitter)
                {
                    // Restaurer l'état du jeu à ce qu'il était avant la pause
                    this.EtatJeu = this.prevEtatJeu;
                }

                // Suspendre les effets sonores au besoin
                this.SuspendreEffetsSonores(this.Quitter);
            }
        }

        /// <summary>
        /// Propriété (accesseur pour menuCourant) retournant ou changeant le menu affiché. Lorsque
        /// sa valeur est null, aucun menu n'est affiché.
        /// </summary>
        /// <value>Menu présentement affiché.</value>
        public Menu MenuCourant
        {
            get
            {
                return this.menuCourant;
            }

            set
            {
                this.menuCourant = value;

                // Mettre le jeu en pause si un menu est affiché
                this.Pause = this.menuCourant != null;
            }
        }

        /// <summary>
        /// Fonction retournant le niveau de résistance aux déplacements en fonction de la couleur du pixel de tuile
        /// à la position donnée.
        /// </summary>
        /// <param name="position">Position du pixel en coordonnées du monde.</param>
        /// <returns>Facteur de résistance entre 0.0f (aucune résistance) et 1.0f (résistance maximale).</returns>
        public float CalculerResistanceAuMouvement(Vector2 position)
        {
            Rectangle joueurRect = new Rectangle((int)this.joueur.PositionPourCollisions.X - 2, (int)this.joueur.PositionPourCollisions.Y - 2, 5, 5);

            foreach (Bloc bloc in this.listeBloc)
            {
                if (joueurRect.Intersects(bloc.AireOccupe))
                {
                    if (this.maBloc != bloc)
                    {
                        this.maBloc = bloc;
                        this.contPousseBloc = 0;
                    }

                    this.contPousseBloc++;
                    if (this.contPousseBloc > 20 && this.ValiderDeplacementBloc(bloc))
                    {
                        this.GestionBloc();
                    }

                    return 1.0f;
                }
            }

            foreach (Porte porte in this.listePorte)
            {
                if (!porte.Ouvert)
                {
                    if (joueurRect.Intersects(porte.Barre))
                    {
                        if (porte.PorteClef && this.joueur.Clef)
                        {
                            porte.Ouvert = true;
                            portesClefOuvert[0] = true;
                            this.boolClef = false;
                            this.joueur.Clef = false;
                        }
                        else
                        {
                            return 1.0f;
                        }
                    }
                }
            }

            foreach (PorteHorizontale porte in this.listePorteHorizontale)
            {
                if (!porte.Ouvert)
                {
                    if (joueurRect.Intersects(porte.Barre))
                    {
                        return 1.0f;
                    }
                }
            }

            // Extraire la couleur du pixel correspondant à la position donnée dans privTuilesCollisions.
            Color pixColor = this.monde.CouleurDeCollision(position);

            // Déterminer le niveau de résistance en fonction de la couleur
            if (pixColor == Color.White)
            {
                return 0.0f;
            }
            else if (pixColor == Color.Blue)
            {
                return 0.9f;
            }
            else
            {
                return 1.0f;
            }
        }

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
        public void ValiderDeplacement(Vector2 posSource, ref int deltaX, ref int deltaY, float resistanceMax)
        {
            Vector2 dest = new Vector2(posSource.X, posSource.Y);

            // Premièrement considérer le déplacement horizontal. Incrémenter la distance horizontale
            // de déplacement jusqu'à deltaX ou jusqu'à ce qu'une résistance supérieure à celle tolérée
            // soit rencontrée.
            while (dest.X != posSource.X + deltaX)
            {
                dest.X += Math.Sign(deltaX);        // incrémenter la distance horizontale

                // Vérifier la résistance
                if (this.CalculerResistanceAuMouvement(dest) > resistanceMax)
                {
                    dest.X -= Math.Sign(deltaX);    // reculer d'un pixel (validé à l'itération précédente)
                    break;
                }
                else if (this.CalculerResistanceAuMouvement(dest) == .9f)
                {                            ///////////******
                    this.joueur.Etat = Personnage.Etats.Tombe;
                }
            }

            // Maintenant considérer le déplacement vertical. Incrémenter la distance verticale
            // de déplacement jusqu'à deltaY ou jusqu'à ce qu'une résistance supérieure à celle tolérée
            // soit rencontrée.
            while (dest.Y != posSource.Y + deltaY)
            {
                dest.Y += Math.Sign(deltaY);        // incrémenter la distance horizontale

                // Vérifier la résistance
                if (this.CalculerResistanceAuMouvement(dest) > resistanceMax)
                {
                    dest.Y -= Math.Sign(deltaY);    // reculer d'un pixel (validé à l'itération précédente)
                    break;
                }
                else if (this.CalculerResistanceAuMouvement(dest) == .9f)
                {
                    this.joueur.Etat = Personnage.Etats.Tombe;
                }
            }

            // Déterminer le déplacement maximal dans les deux directions
            deltaX = (int)(dest.X - posSource.X);
            deltaY = (int)(dest.Y - posSource.Y);
        }

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
        public void ValiderDeplacement1(Vector2 posSource, ref float deltaX, ref float deltaY, float resistanceMax)
        {
            Vector2 dest = new Vector2(posSource.X, posSource.Y);

            // Premièrement considérer le déplacement horizontal. Incrémenter la distance horizontale
            // de déplacement jusqu'à deltaX ou jusqu'à ce qu'une résistance supérieure à celle tolérée
            // soit rencontrée.
            while (dest.X != posSource.X + deltaX)
            {
                dest.X += Math.Sign(deltaX);        // incrémenter la distance horizontale

                // Vérifier la résistance
                if (this.CalculerResistanceAuMouvement(dest) > resistanceMax)
                {
                    dest.X -= Math.Sign(deltaX);    // reculer d'un pixel (validé à l'itération précédente)
                    break;
                }
            }

            // Maintenant considérer le déplacement vertical. Incrémenter la distance verticale
            // de déplacement jusqu'à deltaY ou jusqu'à ce qu'une résistance supérieure à celle tolérée
            // soit rencontrée.
            while (dest.Y != posSource.Y + deltaY)
            {
                dest.Y += Math.Sign(deltaY);        // incrémenter la distance horizontale

                // Vérifier la résistance
                if (this.CalculerResistanceAuMouvement(dest) > resistanceMax)
                {
                    dest.Y -= Math.Sign(deltaY);    // reculer d'un pixel (validé à l'itération précédente)
                    break;
                }
                else if (this.CalculerResistanceAuMouvement(dest) == .9f)
                {
                    this.joueur.Etat = Personnage.Etats.Tombe;
                }
            }

            // Déterminer le déplacement maximal dans les deux directions
            deltaX = (int)(dest.X - posSource.X);
            deltaY = (int)(dest.Y - posSource.Y);
        }

        /// <summary>
        /// Permet au jeu d'effectuer toute initialisation avant de commencer à jouer.
        /// Cette fonction membre peut demander les services requis et charger tout contenu
        /// non graphique pertinent. L'invocation de base.Initialize() itèrera parmi les
        /// composants afin de les initialiser individuellement.
        /// </summary>
        protected override void Initialize()
        {
            // Initialiser la vue de la caméra à la taille de l'écran.
            this.camera = new Camera(new Rectangle(0, 0, this.graphics.GraphicsDevice.Viewport.Width, this.graphics.GraphicsDevice.Viewport.Height));

            // Activer la gestion de services.
            ServiceHelper.Game = this;

            // Activer le service de gestion de l'input. Essayer premièrement
            // d'activer la manette, sinon le clavier
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            if (gamepadState.IsConnected)
            {
                this.Components.Add(new ManetteService(this));
            }
            else
            {
                this.Components.Add(new ClavierService(this));
            }

            this.listeProjectiles = new List<Sprite>();
            this.listeProjectileFini = new List<Sprite>();

            this.listeBloc = new List<Sprite>();
            this.listeBlocFini = new List<Sprite>();

            this.listeOgres = new List<Ennemi>();
            this.listeOgresFini = new List<Ennemi>();

            this.listeSwitch = new List<Sprite>();
            this.listeSwitchFini = new List<Sprite>();

            this.listePorte = new List<Sprite>();
            this.listePorteFini = new List<Sprite>();

            this.listePorteHorizontale = new List<Sprite>();
            this.listePorteHorizontaleFini = new List<Sprite>();

            this.listeVideJoueur = new List<Sprite>();
            this.listeVideJoueurFini = new List<Sprite>();

            this.listeFood = new List<Sprite>();
            this.listeFoodFini = new List<Sprite>();
            this.boolFood = true;

            this.listeClef = new List<Sprite>();
            this.listeClefFini = new List<Sprite>();
            this.boolClef = false;
            this.portesClefOuvert[0] = false;

            // Créer les attributs de gestion des explosions.
            this.randomExplosions = new Random();

            this.randomPJEnemi = new Random();
            this.probPJ = 0.01f;

            // Le jeu est en cours de démarrage. Notez qu'on évite d'exploiter la prorpiété EtatJeu
            // car le setter de cette dernière manipule des effets sonores qui ne sont pas encore
            // chargées par LoadContent()
            this.etatJeu = Etats.Demarrer;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent est invoquée une seule fois par partie et permet de
        /// charger tous vos composants.
        /// </summary>
        protected override void LoadContent()
        {
            // Créer un nouveau SpriteBatch, utilisée pour dessiner les textures.
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            // Charger les images de fonds du jeu pour les différents mondes.
            Map_1_1.LoadContent(this.Content, this.graphics);
            Map_1_2.LoadContent(this.Content, this.graphics);
            Map_1_3.LoadContent(this.Content, this.graphics);
            Map_1_4.LoadContent(this.Content, this.graphics);
            Map_1_5.LoadContent(this.Content, this.graphics);

            // Charger le sprite représentant le joueur.
            Exercice_12_1.Joueur.LoadContent(this.Content, this.graphics);

            // Créer et initialiser le sprite du joueur.
            this.joueur = new Joueur(1150, 300);
            this.joueur.BoundsRect = new Rectangle(0, 0, 600, 600);

            // Imposer la palette de collisions au déplacement du joueur.
            this.joueur.GetValiderDeplacement = this.ValiderDeplacement;

            Projectile.LoadContent(this.Content, this.graphics);

            Bloc.LoadContent(this.Content, this.graphics);

            Ogre.LoadContent(this.Content, this.graphics);

            OgreMouvement.LoadContent(this.Content, this.graphics);

            Switch.LoadContent(this.Content, this.graphics);

            Porte.LoadContent(this.Content, this.graphics);

            PorteHorizontale.LoadContent(this.Content, this.graphics);

            VieDeJoueur.LoadContent(this.Content, this.graphics);

            Food.LoadContent(this.Content, this.graphics);

            Clef.LoadContent(this.Content, this.graphics);

            this.joueur.VieDeJoueur = 10;

            this.joueur.Clef = false;

            // Charger les textures associées aux effets visuels gérées par Game.
            this.explosionParticule = Content.Load<Texture2D>("Textures\\Effets\\explosion");

            this.MondeCourant = Mondes.MAP_1_1;
            this.LoadMap11();

            // Imposer la palette de collisions au déplacement du joueur.                       
            this.joueur.GetResistanceAuMouvement = this.CalculerResistanceAuMouvement;

            // Charger le bruitage de fond du jeu.
            bruitageFond = Content.Load<SoundEffect>("Audio\\Musique\\zelda_3");

            // Charger le bruitage de fond du jeu.
            bruitageFin = Content.Load<SoundEffect>("Audio\\Musique\\zelda_Fin");

            // Charger le bruitage de fond du jeu.
            this.bruitageblock = Content.Load<SoundEffect>("Audio\\Effets\\Bloc\\bloc");

            // Sélectionner et paramétrer le bruitage de fond.
            this.bruitageFondActif = bruitageFond.CreateInstance();
            this.bruitageFondActif.Volume = 0.80f;
            this.bruitageFondActif.IsLooped = true;

            this.bruitageGameOver = Content.Load<SoundEffect>("Audio\\Musique\\GameOver");
            this.bruitageMort = Content.Load<SoundEffect>("Audio\\Effets\\Joueur\\Mort");
            this.bruitageFrapper = Content.Load<SoundEffect>("Audio\\Effets\\Joueur\\Frapper");

            this.bruitageItem = Content.Load<SoundEffect>("Audio\\Effets\\Item\\Item");

            // Sélectionner et paramétrer le bruitage de fond.
            this.bruitageGameOverActif = this.bruitageGameOver.CreateInstance();
            this.bruitageGameOverActif.Volume = 0.80f;
            this.bruitageGameOverActif.IsLooped = false;
            this.bruitageMortActif = this.bruitageMort.CreateInstance();
            this.bruitageMortActif.IsLooped = false;

            // Charger les polices.
            this.policeMessages = Content.Load<SpriteFont>("Polices\\MessagesPolice");
            this.policeMenuTitre = Content.Load<SpriteFont>("Polices\\MenuTitresPolice");
            this.policeMenuItem = Content.Load<SpriteFont>("Polices\\MenuItemsPolice");

            // Charger tous les menus disponibles et les stocker dans la liste des menus.

            // Obtenir une liste des fichiers XML de définition de menu.
            string[] fichiersDeMenu = Directory.GetFiles(Content.RootDirectory + "\\Menus\\");

            // Itérer pour chaque fichier XML trouvé.
            foreach (string nomFichier in fichiersDeMenu)
            {
                // Créer un nouveau menu.
                Menu menu = new Menu();

                // Configurer le nouveau menu à partir de son fichier XML.
                menu.Load(nomFichier);

                // Assigner la fonction déléguée de Game au nouveau menu (pour gestion des
                // sélections de l'usager lors de l'affichage du menu).
                menu.SelectionItemMenu = this.SelectionItemMenu;

                // Ajouter le nouveau menu à la liste des menus du jeu.
                this.listeMenus.Add(menu);
            }

            // Charger les fonds d'écran d'accueil et de générique.
            this.ecranAccueil = Content.Load<Texture2D>("Textures\\SplashDebut");
            this.ecranGenerique = Content.Load<Texture2D>("Textures\\SplashFin");
            this.ecranGameOver = Content.Load<Texture2D>("Textures\\SplashGameOver");
        }

        /// <summary>
        /// Fonction déléguée fournie à tous les menus du jeu pour traiter les sélections 
        /// de l'usager.
        /// </summary>
        /// <param name="nomMenu">Nom du menu d'où provient la sélection.</param>
        /// <param name="item">Item de menu sélectionné.</param>
        protected void SelectionItemMenu(string nomMenu, ItemDeMenu item)
        {
            // Est-ce le menu pour quitter le jeu?
            if (nomMenu == "QuitterMenu")
            {
                // Deux sélections possibles : Oui ou Non
                switch (item.Nom)
                {
                    case "Oui":         // L'usager veut quitter le jeu
                        this.etatJeu = this.EtatJeu = Etats.Quitter;
                        break;

                    case "Non":         // L'usager ne veut pas quitter le jeu
                        this.MenuCourant = null;
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Trouve le menu dont le nom est fourni dans la liste des menus gérée par le jeu.
        /// </summary>
        /// <param name="nomMenu">Nom du menu d'où provient la sélection.</param>
        /// <returns>Menu recherché; null si non trouvé.</returns>
        protected Menu TrouverMenu(string nomMenu)
        {
            // Itérer parmi la liste des menus disponibles
            foreach (Menu menu in this.listeMenus)
            {
                // Si le menu recherché est trouvé, le retourner
                if (menu.Nom == nomMenu)
                {
                    return menu;
                }
            }

            return null;    // aucun menu correspondant au fourni
        }

        /// <summary>
        /// UnloadContent est invoquée une seule fois par partie et permet de
        /// libérer vos composants.
        /// </summary>
        protected override void UnloadContent()
        {
            // À FAIRE: Libérez tout contenu de ContentManager ici
        }

        /// <summary>
        /// Permet d'implanter les comportements logiques du jeu tels que
        /// la mise à jour du monde, la détection de collisions, la lecture d'entrées
        /// et les effets audio.
        /// </summary>
        /// <param name="gameTime">Fournie un instantané du temps de jeu.</param>
        protected override void Update(GameTime gameTime)
        {
            // Si le jeu est en cours de démarrage, passer à l'état de jouer.
            if (this.EtatJeu == Etats.Demarrer)
            {
                // L'usager veut-il démarrer la partie? 
                if (ServiceHelper.Get<IInputService>().Sauter(1))
                {
                    this.EtatJeu = Etats.Jouer;

                    base.Update(gameTime);
                    return;
                }

                // Rien d'autre à faire alors on quitte la fonction 
                base.Update(gameTime);
                return;
            }

            // Si le jeu est en cours de démarrage, passer à l'état de jouer.
            if (this.EtatJeu == Etats.GameOver)
            {
                if (ServiceHelper.Get<IInputService>().Sauter(1))
                {
                    // Bruitage de fond.
                    if (this.bruitageGameOverActif.State == SoundState.Playing)
                    {
                        this.bruitageGameOverActif.Stop();
                    }

                    this.contMort = 0;
                    this.GameOverState = false;
                    this.EtatJeu = Etats.Demarrer;
                    this.joueur = new Joueur(1150, 300);
                    this.joueur.BoundsRect = new Rectangle(0, 0, 600, 600);
                    this.joueur.GetValiderDeplacement = this.ValiderDeplacement;
                    this.joueur.VieDeJoueur = 10;
                    this.MondeCourant = Mondes.MAP_1_1;
                    this.boolClef = false;
                    this.boolFood = false;
                    this.joueur.Clef = false;
                    this.ClearMap();
                    this.LoadMap11();
                }

                if (ServiceHelper.Get<IInputService>().Quitter(1))
                {
                    // Bruitage de fond.
                    if (this.bruitageGameOverActif.State == SoundState.Playing)
                    {
                        this.bruitageGameOverActif.Stop();
                    }

                    this.Quitter = !this.Quitter;
                }

                // Rien d'autre à faire alors on quitte la fonction 
                base.Update(gameTime);
                return;
            }

            // Si le jeu est en cours de terminaison, l'écran de générique demeure affichée
            // jusqu'à ce que le générique soit terminé ou que l'usager presse la touche
            // appropriée
            if (this.EtatJeu == Etats.Quitter)
            {
                if (this.bruitageFinOn != true)
                {
                    bruitageFin.Play();
                    this.bruitageFinOn = true;
                }

                // L'usager veut-il quitter immédiatement
                if (ServiceHelper.Get<IInputService>().Sauter(0))
                {
                    this.Exit();
                }

                // Modifier la position du générique de sorte qu'il monte lentement vers le haut
                this.generiqueScrollPos--;

                // Rien d'autre à faire alors on quitte la fonction
                base.Update(gameTime);
                return;
            }

            // Un menu est-il affiché?
            if (this.MenuCourant != null)
            {
                // Oui, alors gérer les inputs pour ce menu.
                this.MenuCourant.GetInput(gameTime);

                // Lorsqu'un menu est affiché, le jeu est en pause alors il n'y a rien d'autre
                // à faire dans Update().
                base.Update(gameTime);
                return;
            }

            // Permettre de quitter via le clavier.
            if (ServiceHelper.Get<IInputService>().Quitter(1))
            {
                this.MenuCourant = this.TrouverMenu("QuitterMenu");
            }

            // Est-ce que le bouton de pause a été pressé?
            if (ServiceHelper.Get<IInputService>().Pause(1))
            {
                this.Pause = !this.Pause;
            }

            // Si le jeu est en pause, interrompre la mise à jour.
            if (this.Pause)
            {
                base.Update(gameTime);
                return;
            }

            // S'assurer que le bruitage de fond est en lecture.
            if (this.bruitageFondActif.State == SoundState.Stopped)
            {
                this.bruitageFondActif.Play();
                this.bruitageFondActif.Volume = 0.1f;
            }

            // Bruitage de fond.
            if (this.bruitageFondActif.State == SoundState.Paused)
            {
                this.bruitageFondActif.Play();
            }

            // Mettre à jour les particules d'explosion
            this.UpdateParticulesExplosions(gameTime);

            // Appelé pour arrêter le jeu aprés l'animation de mourir.
            if (this.joueur.Etat == Personnage.Etats.Mort && this.joueur.IndexTuile == 4)
            {
                if (this.contMort++ > 150)
                {
                    this.bruitageGameOverActif.Play();
                    this.GameOverState = !this.GameOverState;
                }

                base.Update(gameTime);
                return;
            }

            // Mettre à jour le sprite du joueur puis centrer la camera sur celui-ci.
            this.joueur.Update(gameTime, this.graphics);

            if (this.joueur.CouleurCollison <= 99)
            {
                this.joueur.CouleurCollison++;
            }

            if (this.joueur.VieDeJoueur == 0)
            {
                this.joueur.CouleurCollison = 100;
                
                if (this.bruitageMortActif.State != SoundState.Playing && this.contMort == 0)
                {
                    this.bruitageMort.Play();
                }

                this.bruitageFondActif.Stop();

                this.joueur.SuspendreEffetsSonores(true);

                switch ((gameTime.TotalGameTime.Milliseconds / 4) % 4)
                {
                    case 0:
                        this.joueur.Direction = Personnage.Directions.Nord;
                        break;
                    case 1:
                        this.joueur.Direction = Personnage.Directions.Est;
                        break;
                    case 2:
                        this.joueur.Direction = Personnage.Directions.Sud;
                        break;
                    default:
                        this.joueur.Direction = Personnage.Directions.Ouest;
                        break;
                }

                if (this.contMort++ > 110)
                {
                    this.joueur.Etat = Personnage.Etats.Mort;
                }

                this.bruitageFondActif.Pause();
                base.Update(gameTime);
                return;
            }

            if (this.joueur.Etat == Personnage.Etats.Tombe && this.joueur.ContTombe > 60)
            {
                this.joueur.AngleRotation = this.joueur.ContTombe = 0;
                this.joueur.Etat = Personnage.Etats.Stationnaire;
                this.joueur.Direction = Personnage.Directions.Nord;
                this.joueur.Echelle = 1.0f;
                this.joueur.Position = this.monde.PositionInitiale;
                this.joueur.VieDeJoueur--;
            }

            this.camera.Centrer(this.joueur.Position);

            // Mettre à jour les ogres.
            foreach (Ennemi ogre in this.listeOgres)
            {
                ogre.SeTournerVers(this.joueur.Position);

                // Déterminer si on doit créer un nouvel astéroide.
                if (this.randomPJEnemi.NextDouble() < this.probPJ)
                {
                    // Créer le sprite
                    if (ogre.Direction == Personnage.Directions.Nord)
                    {
                        Projectile pj = new Projectile(new Vector2(ogre.Position.X, ogre.Position.Y - 60), 0);
                        pj.TypeProjectile = Projectile.TypesProjectiles.Ennemi;
                        this.listeProjectiles.Add(pj);
                    }

                    // Créer le sprite
                    if (ogre.Direction == Personnage.Directions.NordEst)
                    {
                        Projectile pj = new Projectile(new Vector2(ogre.Position.X + 60, ogre.Position.Y - 60), 1);
                        pj.TypeProjectile = Projectile.TypesProjectiles.Ennemi;
                        this.listeProjectiles.Add(pj);
                    }

                    // Créer le sprite
                    if (ogre.Direction == Personnage.Directions.Est)
                    {
                        Projectile pj = new Projectile(new Vector2(ogre.Position.X + 60, ogre.Position.Y), 2);
                        pj.TypeProjectile = Projectile.TypesProjectiles.Ennemi;
                        this.listeProjectiles.Add(pj);
                    }

                    // Créer le sprite
                    if (ogre.Direction == Personnage.Directions.SudEst)
                    {
                        Projectile pj = new Projectile(new Vector2(ogre.Position.X + 60, ogre.Position.Y + 60), 3);
                        pj.TypeProjectile = Projectile.TypesProjectiles.Ennemi;
                        this.listeProjectiles.Add(pj);
                    }

                    // Créer le sprite
                    if (ogre.Direction == Personnage.Directions.Sud)
                    {
                        Projectile pj = new Projectile(new Vector2(ogre.Position.X, ogre.Position.Y + 60), 4);
                        pj.TypeProjectile = Projectile.TypesProjectiles.Ennemi;
                        this.listeProjectiles.Add(pj);
                    }

                    // Créer le sprite
                    if (ogre.Direction == Personnage.Directions.SudOuest)
                    {
                        Projectile pj = new Projectile(new Vector2(ogre.Position.X - 60, ogre.Position.Y + 60), 5);
                        pj.TypeProjectile = Projectile.TypesProjectiles.Ennemi;
                        this.listeProjectiles.Add(pj);
                    }

                    // Créer le sprite
                    if (ogre.Direction == Personnage.Directions.Ouest)
                    {
                        Projectile pj = new Projectile(new Vector2(ogre.Position.X - 60, ogre.Position.Y), 6);
                        pj.TypeProjectile = Projectile.TypesProjectiles.Ennemi;
                        this.listeProjectiles.Add(pj);
                    }

                    // Créer le sprite
                    if (ogre.Direction == Personnage.Directions.NordOuest)
                    {
                        Projectile pj = new Projectile(new Vector2(ogre.Position.X - 60, ogre.Position.Y - 60), 7);
                        pj.TypeProjectile = Projectile.TypesProjectiles.Ennemi;
                        this.listeProjectiles.Add(pj);
                    }
                }

                if (ogre.Collision(this.joueur) && this.joueur.CouleurCollison > 99)
                {
                    this.joueur.VieDeJoueur--;
                    this.joueur.CouleurCollison = 0;
                    this.bruitageFrapper.Play();
                }

                ogre.Update(gameTime, this.graphics);
            }

            // Se débarasser des astéroïdes ayant quitté l'écran.
            foreach (Ennemi ogre in this.listeOgresFini)
            {
                this.listeOgres.Remove(ogre);
            }

            this.GestionProjectile(gameTime);

            this.GestionSwtich(gameTime);

            this.UpdateVieDeJoueur();

            foreach (Food food in this.listeFood)
            {
                if (this.joueur.Collision(food))
                {
                    this.boolFood = false;
                    this.joueur.VieDeJoueur = 10;
                    this.listeFoodFini.Add(food);
                    this.bruitageItem.Play();
                }
            }

            foreach (Food food in this.listeFoodFini)
            {
                this.listeFood.Remove(food);
            }

            foreach (Clef clef in this.listeClef)
            {
                if (this.joueur.Collision(clef))
                {
                    this.boolClef = true;
                    this.joueur.Clef = true;
                    this.listeClefFini.Add(clef);
                    this.bruitageItem.Play();

                    Clef clef1 = new Clef(600 - 16, 15);
                    this.listeClef.Add(clef1);
                    break;
                }
            }

            foreach (Clef clef in this.listeClefFini)
            {
                this.listeClef.Remove(clef);
            }

            foreach (Projectile pj in this.listeProjectiles)
            {
                pj.Update(gameTime, this.graphics);
            }

            foreach (Bloc bloc in this.listeBloc)
            {
                if (bloc.VideDeBloc > 0)
                {
                    bloc.Update(gameTime, this.graphics);
                }
            }

            foreach (Bloc bloc in this.listeBloc)
            {
                this.listeBlocFini.Add(bloc);
            }

            foreach (Bloc bloc in this.listeBlocFini)
            {
                if (bloc.BlocEchelle <= 0.05f)
                {
                    this.listeBloc.Remove(bloc);
                }
            }

            this.GestionAtteintUneSortie();

            base.Update(gameTime);
        }

        /// <summary>
        /// Suspend temporairement (pause) ou réactive les effets sonores du jeu.
        /// </summary>
        /// <param name="suspendre">Indique si les effets sonores doivent être suspendus ou réactivés.</param>
        protected void SuspendreEffetsSonores(bool suspendre)
        {
            // Suspendre au besoin les effets sonores du vaisseau.
            this.joueur.SuspendreEffetsSonores(suspendre);

            // Suspendre au besoin les effets sonores des ogres.
            foreach (Ennemi ogre in this.listeOgres)
            {
                ogre.SuspendreEffetsSonores(suspendre);
            }

            if (suspendre)
            {
                // Bruitage de fond.
                if (this.bruitageFondActif.State == SoundState.Playing)
                {
                    this.bruitageFondActif.Pause();
                }
            }
            else
            {
                // Bruitage de fond.
                if (this.bruitageFondActif.State == SoundState.Paused)
                {
                    this.bruitageFondActif.Play();
                }
            }
        }

        /// <summary>
        /// Routine mettant à jour les particules d'explosions. Elle s'occupe de:
        ///   1 - Mettre à jour chaque particule
        ///   2 - Éliminer les particules n'étant plus visibles.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected void UpdateParticulesExplosions(GameTime gameTime)
        {
            // Liste de particules à détruire
            List<ParticuleExplosion> particulesFinies = new List<ParticuleExplosion>();

            // Mettre à jour les particules d'explosion
            foreach (ParticuleExplosion particule in this.listeParticulesExplosions)
            {
                particule.Update(gameTime, this.graphics);

                // Si la particule est devenue invisible, alors on peut l'ignorer à partir
                // de maintenant
                if (!particule.Visible)
                {
                    particulesFinies.Add(particule);
                }
            }

            // Éliminer les particules ayant disparu de l'écran
            foreach (ParticuleExplosion particule in particulesFinies)
            {
                this.listeParticulesExplosions.Remove(particule);
            }
        }

        /// <summary>
        /// Cette fonction membre est invoquée lorsque le jeu doit mettre à jour son 
        /// affichage.
        /// </summary>
        /// <param name="gameTime">Fournie un instantané du temps de jeu.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Activer le blending alpha (pour la transparence des sprites).
            this.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            // Si le jeu est en état de démarrage, afficher l'écran d'accueil 
            if (this.EtatJeu == Etats.Demarrer)
            {
                this.DrawEcranAccueil(this.spriteBatch);
                this.spriteBatch.End();
                base.Draw(gameTime);
                return;
            }

            // Si le jeu est en état de démarrage, afficher l'écran d'accueil 
            if (this.EtatJeu == Etats.GameOver)
            {
                FinDePartie();
                this.DrawEcranGameOver(this.spriteBatch);
                this.spriteBatch.End();
                base.Draw(gameTime);
                return;
            }

            // Si le jeu est en état de démarrage, afficher l'écran d'accueil 
            if (this.EtatJeu == Etats.Demarrer)
            {
                this.DrawEcranAccueil(this.spriteBatch);
                this.spriteBatch.End();
                base.Draw(gameTime);
                return;
            }

            // En phase de terminaison, alors afficher le générique 
            if (this.EtatJeu == Etats.Quitter)
            {
                this.DrawGenerique(this.spriteBatch);

                // Ignorer le reste de la fonction d'affichage 
                this.spriteBatch.End();
                base.Draw(gameTime);
                return;
            }

            // Afficher les tuiles d'arrière-plan.
            this.monde.Draw(this.camera, this.spriteBatch);

            // Liste statique exploitée pour ordonner les sprites avant leur affichage.
            List<Sprite> listeDraw = new List<Sprite>();

            // Ajouter les ogres à afficher à la liste.
            // listeDraw.Add(this.joueur);
            foreach (Ennemi ogre in this.listeOgres)
            {
                if (ogre is OgreMouvement)
                {
                    this.spriteBatch.Draw(
                            ogre.Texture,               // texture
                            ogre.Position,              // position
                            null,                       // sourceRectangle
                            Color.White,                // couleur
                            0,  // angle de rotation
                            new Vector2(16, 16),
                            2f,                         // échelle d'affichage
                            SpriteEffects.None,         // effets
                            0.0f);
                }
                else
                {
                    this.spriteBatch.Draw(
                            ogre.Texture,               // texture
                            ogre.Position,              // position
                            null,                       // sourceRectangle
                            Color.White,                // couleur
                            0,                          // angle de rotation
                            new Vector2(16, 16),
                            0.75f,                      // échelle d'affichage
                            SpriteEffects.None,         // effets
                            0.0f);
                }
            }

            // Afficher les blocs.
            foreach (Bloc bloc in this.listeBloc)
            {
                // Extraire la couleur du pixel correspondant à la position donnée dans privTuilesCollisions.
                Color pixColor = this.monde.CouleurDeCollision(bloc.Position);
                if (pixColor == Color.Blue)
                {
                    this.spriteBatch.Draw(
                    bloc.Texture,               // texture
                    bloc.Position,              // position
                    null,                       // sourceRectangle
                    Color.White,                // couleur
                    (float)gameTime.TotalGameTime.TotalMilliseconds / 360,  // angle de rotation
                    new Vector2(16, 16),
                    bloc.BlocEchelle,           // échelle d'affichage
                    SpriteEffects.None,         // effets
                    0.0f);

                    bloc.BlocEchelle -= 0.02f;
                }
                else
                {
                    listeDraw.Add(bloc);
                }
            }

            foreach (Switch switcht1 in this.listeSwitch)
            {
                listeDraw.Add(switcht1);
            }

            foreach (Porte porte in this.listePorte)
            {
                listeDraw.Add(porte);
            }

            foreach (PorteHorizontale porte in this.listePorteHorizontale)
            {
                listeDraw.Add(porte);
            }

            // Afficher les projectiles.
            foreach (Projectile pj in this.listeProjectiles)
            {
                if (pj.TypeProjectile == Projectile.TypesProjectiles.Joueur)
                {
                    this.spriteBatch.Draw(
                        pj.Texture,                 // texture
                        pj.Position,                // position
                        null,                       // sourceRectangle
                        Color.Chartreuse,           // couleur
                        0,                          // angle de rotation
                        new Vector2(16, 16),
                        .7f + (((gameTime.TotalGameTime.Milliseconds / 60) % 3) / 3f),             // échelle d'affichage
                        SpriteEffects.None,         // effets
                        0.0f);                      // profondeur de couche (layer depth));
                }
                else
                {
                    this.spriteBatch.Draw(
                        pj.Texture,                 // texture
                        pj.Position,                // position
                        null,                       // sourceRectangle
                        Color.LightSalmon,                // couleur
                        0,                          // angle de rotation
                        new Vector2(16, 16),
                        .7f + (((gameTime.TotalGameTime.Milliseconds / 60) % 3) / 3f),             // échelle d'affichage
                        SpriteEffects.None,         // effets
                        0.0f);                      // profondeur de couche (layer depth));
                }
            }

            // Trier les sprite en ordre croissant de position verticale, puis les afficher.
            listeDraw.Sort(ComparerSpritesPourAffichage);
            foreach (Sprite sprite in listeDraw)
            {
                sprite.Draw(this.camera, this.spriteBatch);
            }

            if (((this.joueur.CouleurCollison > 0 && this.joueur.CouleurCollison < 10)
                || (this.joueur.CouleurCollison > 20 && this.joueur.CouleurCollison < 30)
                || (this.joueur.CouleurCollison > 40 && this.joueur.CouleurCollison < 50)
                || (this.joueur.CouleurCollison > 60 && this.joueur.CouleurCollison < 70)
                || (this.joueur.CouleurCollison > 80 && this.joueur.CouleurCollison < 90))
                 && (this.joueur.Etat != Personnage.Etats.Tombe))
            {
                // Afficher le joueur en etat de tombe
                this.spriteBatch.Draw(
                    this.joueur.Texture,             // texture
                    new Vector2(this.joueur.Position.X - 16, this.joueur.Position.Y - 16),
                    null,                       // sourceRectangle
                    Color.Crimson,                // couleur
                    0,  // angle de rotation
                    new Vector2(16, 16),        // origine de rotation
                    this.joueur.Echelle,             // échelle d'affichage
                    SpriteEffects.None,         // effets
                    0.0f);                      // profondeur de couche (layer depth)
            }
            else if (this.joueur.Etat != Personnage.Etats.Tombe)
            {
                this.joueur.Draw(this.camera, this.spriteBatch);
            }
            else
            {
                // Afficher le joueur en etat de tombe
                this.spriteBatch.Draw(
                    this.joueur.Texture,             // texture
                    this.joueur.Position,            // position
                    null,                       // sourceRectangle
                    Color.White,                // couleur
                    this.joueur.AngleRotation,  // angle de rotation
                    new Vector2(16, 16),        // origine de rotation
                    this.joueur.Echelle,             // échelle d'affichage
                    SpriteEffects.None,         // effets
                    0.0f);                      // profondeur de couche (layer depth)
            }          

            // Afficher le menu courant s'il y en a un sélectionné.
            if (this.MenuCourant != null)
            {
                // Dessiner le menu.
                this.MenuCourant.Draw(
                    this.spriteBatch,
                    this.policeMenuTitre,
                    this.policeMenuItem,
                    this.couleurMenuTitre,
                    this.couleurMenuItem,
                    this.couleurMenuItemSelectionne);
            }

            // Afficher les explosions
            foreach (ParticuleExplosion particule in this.listeParticulesExplosions)
            {
                particule.Draw(this.spriteBatch);
            }

            foreach (Clef clef in this.listeClef)
            {
                clef.Draw(this.spriteBatch);
            }

            foreach (Food food in this.listeFood)
            {
                food.Draw(this.spriteBatch);
            }

            foreach (Sprite vie in this.listeVideJoueur)
            {
                vie.Draw(this.spriteBatch);
            }

            // Si le jeu est en état de démarrage, afficher l'écran d'accueil 
            if (this.EtatJeu == Etats.Pause)
            {
                this.DrawEcranPause(this.spriteBatch);
                this.spriteBatch.End();
                base.Draw(gameTime);
                return;
            }          

            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Routine d'affichage de message fourni, centré à l'écran par défaut.
        /// </summary>
        /// <param name="spriteBatch">Tampon d'affichage.</param>
        /// <param name="message">Message à afficher.</param>
        /// <param name="decalageY">Décalage vertical (en pixels) par rapport au centre de l'écran.</param>
        /// <param name="couleur">Couleur du texte.</param>
        protected void DrawMessage(SpriteBatch spriteBatch, string message, int decalageY, Color couleur)
        {
            // Afficher le message s'il y a lieu.
            if (message.Length > 0)
            {
                // L'origine du message sera positionnée au centre de l'écran.
                Vector2 centreEcran = new Vector2(
                    this.graphics.GraphicsDevice.Viewport.Width / 2,
                    this.graphics.GraphicsDevice.Viewport.Height / 2);

                // Appliquer le décalage vertical par rapport au centre de l'écran.
                centreEcran.Y += decalageY;

                // Splitter le message en fonction des \n dans celui-ci, afin de center chaque chaîne du message
                // à l'écran.
                string[] lignes = message.Split('\n');

                // Afficher chaque chaîne du message indiciduellement, centrées l'une sous l'autre.
                for (int idx = 0; idx < lignes.Length; idx++)
                {
                    // L'origine d'affichage de la chaîne est son point central.
                    Vector2 centrePolice = this.policeMessages.MeasureString(lignes[idx]) / 2;

                    // Afficher la chaîne centré à l'écran.
                    spriteBatch.DrawString(
                        this.policeMessages,        // police d'affichge
                        lignes[idx],                // message à afficher
                        centreEcran,                // position où afficher le message
                        couleur,                    // couleur du texte
                        0,                          // angle de rotation
                        centrePolice,               // origine du texte (centrePolice positionné à centreEcran)
                        1.0f,                       // échelle d'affichage
                        SpriteEffects.None,         // effets
                        1.0f);                      // profondeur de couche (layer depth)

                    // Décaler afin que la chaîne soit positionnée sous la précédente.
                    centreEcran.Y += centrePolice.Y * 2;
                }
            }
        }

        /// <summary>
        /// Fonction dessinant l'écran d'accueil. Un message est affiché sur la texture d'accueil
        /// afin d'indiquer à l'usager quelle touche presser pour démarrer la partie.
        /// </summary>
        /// <param name="spriteBatch">Tampon d'affichage.</param>
        protected void DrawEcranAccueil(SpriteBatch spriteBatch)
        {
            // Dessiner le fond d'écran.
            spriteBatch.Draw(this.ecranAccueil, Vector2.Zero, Color.White);

            // Afficher le message approprié selon le périphérique d'inputs.
            string message = string.Empty;
            if (ServiceHelper.Get<IInputService>().GetType() == typeof(ClavierService))
            {
                message = "Pressez Espace pour commencer...";
            }
            else if (ServiceHelper.Get<IInputService>().GetType() == typeof(ManetteService))
            {
                message = "Pressez A pour commencer...";
            }
            else
            {
                message = "ERREUR: aucune manette ou clavier!";
            }

            // Afficher le message 50 pixels plus bas que le centre de l'écran.
            this.DrawMessage(this.spriteBatch, message, 50, Color.Blue);
        }

        /// <summary>
        /// Fonction dessinant l'écran du générique de fin de partie. Les chaînes du générique défilent
        /// verticalement du bas de l'écran vers le haut, tel un générique de fin de film. Chaque chaîne
        /// est centrée horizontalement à l'écran.
        /// </summary>
        /// <param name="spriteBatch">Tampon d'affichage.</param>
        protected void DrawGenerique(SpriteBatch spriteBatch)
        {
            const int Espacement = 50;      // espace entre chaque groupe de lignes

            // Dessiner le fond d'écran.
            this.spriteBatch.Draw(this.ecranGenerique, Vector2.Zero, Color.White);

            // Groupes de lignes constituant le générique. Chaque élément du tableau est un groupe
            // de lignes inclu dans la même string mais séparées de \n.
            string[] lignes = 
            {
                 "Matthieu Doell\nAlexandre Grenier",
                 "Cours 20884 IFM\nPhysique du Jeu", 
                 "Ce jeu enfreint plusieurs droits d'auteurs\nRedistribution interdite",
                 "Copyright 2012\nPRODUCTIONS DOELL-GRENIER\nNintendo"
            };

            // Position verticale (en pixels) où est affichée le groupe de lignes courant dans la
            // boucle suivante.
            int row = this.generiqueScrollPos;

            // Afficher chaque groupe de lignes du générique.
            for (int idx = 0; idx < lignes.Length; idx++)
            {
                this.DrawMessage(this.spriteBatch, lignes[idx], row, Color.Pink);

                // Mettre à jour la position de la prochaine ligne du générique.
                row += (int)this.policeMessages.MeasureString(lignes[idx]).Y + Espacement;
            }

            // Si la dernière ligne du générique a disparue au haut de l'écran, on termine le
            // programme.
            if (row - Espacement < -(this.graphics.GraphicsDevice.Viewport.Height / 2))
            {
                this.DrawMessage(this.spriteBatch, "Merci", 290, Color.Pink);
            }
        }

        /// <summary>
        /// Fonction dessinant l'écran d'accueil. Un message est affiché sur la texture d'accueil
        /// afin d'indiquer à l'usager quelle touche presser pour démarrer la partie.
        /// </summary>
        /// <param name="spriteBatch">Tampon d'affichage.</param>
        protected void DrawEcranGameOver(SpriteBatch spriteBatch)
        {
            // Dessiner le fond d'écran.
            spriteBatch.Draw(this.ecranGameOver, Vector2.Zero, Color.White);

            // Afficher le message approprié selon le périphérique d'inputs.
            string message = string.Empty;
            if (ServiceHelper.Get<IInputService>().GetType() == typeof(ClavierService))
            {
                message = "Pressez Espace pour commencer...\nPressez ESC pour quitter";
            }
            else if (ServiceHelper.Get<IInputService>().GetType() == typeof(ManetteService))
            {
                message = "Pressez A pour commencer...\nPressez sur Start pour quitter";
            }
            else
            {
                message = "ERREUR: aucune manette ou clavier!";
            }

            // Afficher le message 50 pixels plus bas que le centre de l'écran.
            this.DrawMessage(this.spriteBatch, message, 50, Color.White);
        }

        /// <summary>
        /// Fonction dessinant l'écran d'accueil. Un message est affiché sur la texture d'accueil
        /// afin d'indiquer à l'usager quelle touche presser pour démarrer la partie.
        /// </summary>
        /// <param name="spriteBatch">Tampon d'affichage.</param>
        protected void DrawEcranPause(SpriteBatch spriteBatch)
        {
            // Dessiner le fond d'écran.
            //spriteBatch.Draw(this.ecranGameOver, Vector2.Zero, Color.White);

            // Afficher le message approprié selon le périphérique d'inputs.
            string message = string.Empty;
            if (ServiceHelper.Get<IInputService>().GetType() == typeof(ClavierService))
            {
                message = "Pressez Z pour continuer";
            }
            else if (ServiceHelper.Get<IInputService>().GetType() == typeof(ManetteService))
            {
                message = "Pressez Start pour continuer";
            }
            else
            {
                message = "ERREUR: aucune manette ou clavier!";
            }

            // Afficher le message 50 pixels plus bas que le centre de l'écran.
            this.DrawMessage(this.spriteBatch, message, 50, Color.White);
        }

        /// <summary>
        /// Fonction comparative pour trier des sprites en ordre croissant de position verticale dans le monde.
        /// Cette fonction est exploitée par Draw pour afficher les sprites de façon à ce que ceux plus bas
        /// dans le monde soient considérés comme "devant" ceux plus haut.
        /// </summary>
        /// <param name="s1">Premier sprite à comparer.</param>
        /// <param name="s2">Second sprite à comparer.</param>
        /// <returns>-1, 0 ou 1 selon la position verticale relative des deux sprites.</returns>
        private static int ComparerSpritesPourAffichage(Sprite s1, Sprite s2)
        {
            if (s1.Position.Y < s2.Position.Y)
            {
                return -1;
            }
            else if (s1.Position.Y > s2.Position.Y)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Fonction qui verifie si le joueur a atteint une sortit            
        /// </summary>
        private void GestionAtteintUneSortie()
        {
            // Vérifier si le joueur a atteint une sortie du monde.
            if (this.monde.AtteintUneSortie(this.joueur))
            {
                if (this.MondeCourant == Mondes.MAP_1_1)
                {
                    this.MondeCourant = Mondes.MAP_1_2;
                    this.joueur.Position = new Vector2(300, 490);

                    this.LoadMap12();
                }
                else if (this.MondeCourant == Mondes.MAP_1_2)
                {
                    if (this.joueur.Position.Y > 500)
                    {
                        this.MondeCourant = Mondes.MAP_1_1;
                        this.joueur.Position = new Vector2(300, 60);

                        this.LoadMap11();
                    }
                    else if (this.joueur.Position.Y < 60)
                    {
                        this.MondeCourant = Mondes.MAP_1_3;
                        this.joueur.Position = new Vector2(300, 490);

                        this.LoadMap13();
                    }
                    else if (this.joueur.Position.X > 510)
                    {
                        this.MondeCourant = Mondes.MAP_1_4;
                        this.joueur.Position = new Vector2(97, 300);

                        this.LoadMap14();
                    }
                    else if (this.joueur.Position.X < 85)
                    {
                        this.MondeCourant = Mondes.MAP_1_5;
                        this.joueur.Position = new Vector2(500, 285);

                        this.LoadMap15();
                    }
                }
                else if (this.MondeCourant == Mondes.MAP_1_3)
                {
                    this.MondeCourant = Mondes.MAP_1_2;
                    this.joueur.Position = new Vector2(300, 80);

                    this.LoadMap12();
                }
                else if (this.MondeCourant == Mondes.MAP_1_4)
                {
                    this.MondeCourant = Mondes.MAP_1_2;
                    this.joueur.Position = new Vector2(500, 280);

                    this.LoadMap12();
                }
                else if (this.MondeCourant == Mondes.MAP_1_5)
                {
                    if (this.joueur.Position.X > 510)
                    {
                        this.MondeCourant = Mondes.MAP_1_2;
                        this.joueur.Position = new Vector2(90, 290);
                        this.LoadMap12();
                    }
                    else if (this.joueur.Position.Y < 60)
                    {
                        this.Quitter = !this.Quitter;
                    }
                }
            }
        }

        /// <summary>
        /// Fonction qui parcourt les listes et clear les item pour reinitialiser les cartes            
        /// </summary>
        private void ClearMap()
        {
            foreach (Bloc bloc in this.listeBloc)
            {
                this.listeBlocFini.Add(bloc);
            }

            foreach (Bloc bloc in this.listeBlocFini)
            {
                this.listeBloc.Remove(bloc);
            }

            foreach (Projectile pj in this.listeProjectiles)
            {
                this.listeProjectileFini.Add(pj);
            }

            foreach (Projectile pj in this.listeProjectileFini)
            {
                this.listeProjectiles.Remove(pj);
            }

            foreach (Ennemi ogre in this.listeOgres)
            {
                this.listeOgresFini.Add(ogre);
            }

            foreach (Ennemi ogre in this.listeOgresFini)
            {
                this.listeOgres.Remove(ogre);
            }

            foreach (Switch switch1 in this.listeSwitch)
            {
                this.listeSwitchFini.Add(switch1);
            }

            foreach (Switch switch1 in this.listeSwitchFini)
            {
                this.listeSwitch.Remove(switch1);
            }

            foreach (Porte porte in this.listePorte)
            {
                this.listePorteFini.Add(porte);
            }

            foreach (Porte porte in this.listePorteFini)
            {
                this.listePorte.Remove(porte);
            }

            foreach (PorteHorizontale porte in this.listePorteHorizontale)
            {
                this.listePorteHorizontaleFini.Add(porte);
            }

            foreach (PorteHorizontale porte in this.listePorteHorizontaleFini)
            {
                this.listePorteHorizontale.Remove(porte);
            }

            foreach (Food food in this.listeFood)
            {
                this.listeFoodFini.Add(food);
            }

            foreach (Food food in this.listeFoodFini)
            {
                this.listeFood.Remove(food);
            }

            foreach (Clef clef in this.listeClef)
            {
                this.listeClefFini.Add(clef);
            }

            foreach (Clef clef in this.listeClefFini)
            {
                this.listeClef.Remove(clef);
            }

            if (this.boolClef == true)
            {
                Clef clef1 = new Clef(600 - 16, 15);
                this.listeClef.Add(clef1);
            }
        }


        /// <summary>
        /// Remet les valeurs au conditions initiales pour la nouvelle partie.
        /// </summary>
        private void FinDePartie()
        {
            this.boolFood = true;
            this.boolClef = false;
            for (int i = 0; i < portesClefOuvert.Length; i++)
            {
                this.portesClefOuvert[i] = false;
            }
        }

        /// <summary>
        /// Fonction qui load tout les elements de map 1-1           
        /// </summary>
        private void LoadMap11()
        {
            this.ClearMap();

            Bloc bloc0 = new Bloc(300, 105);
            bloc0.BoundsRect = new Rectangle(258, 63, 84, 84);

            this.listeBloc.Add(bloc0);

            OgreMouvement ogre = new OgreMouvement(new Vector2(400, 300));
            ogre.BoundsRect = new Rectangle(91, 91, 415, 415);
            this.listeOgres.Add(ogre);
        }

        /// <summary>
        /// Fonction qui load tout les elements de map 1-2           
        /// </summary>
        private void LoadMap12()
        {
            this.ClearMap();

            Bloc bloc0 = new Bloc(106, 272);
            bloc0.BoundsRect = new Rectangle(64, 230, 84, 84);

            Bloc bloc1 = new Bloc(134, 300);
            bloc1.BoundsRect = new Rectangle(92, 258, 84, 84);

            Bloc bloc2 = new Bloc(106, 328);
            bloc2.BoundsRect = new Rectangle(64, 286, 84, 84);

            this.listeBloc.Add(bloc0);
            this.listeBloc.Add(bloc1);
            this.listeBloc.Add(bloc2);

            Ogre ogre = new Ogre(new Vector2(152, 144));
            this.listeOgres.Add(ogre);
            Ogre ogre1 = new Ogre(new Vector2(444, 144));
            this.listeOgres.Add(ogre1);

            OgreMouvement ogre2 = new OgreMouvement(new Vector2(350, 350));
            this.listeOgres.Add(ogre2);
            ogre2.BoundsRect = new Rectangle(300, 300, 193, 215);

            Switch switch1 = new Switch(134, 272);
            this.listeSwitch.Add(switch1);
            switch1.Type = Switch.Types.Nord;

            Switch switch2 = new Switch(162, 300);
            this.listeSwitch.Add(switch2);
            switch2.Type = Switch.Types.Est;

            Porte porteNord = new Porte(300, 75, Porte.Directions.Nord);
            porteNord.Ouvert = true; // fermé au prochaine Update, invoque le son de fermeture
            this.listePorte.Add(porteNord);

            PorteHorizontale porteEst = new PorteHorizontale(525, 300, PorteHorizontale.Directions.Est);
            porteEst.Ouvert = true; // fermé au prochaine Update, invoque le son de fermeture
            porteEst.Direction = PorteHorizontale.Directions.Est;
            this.listePorteHorizontale.Add(porteEst);
        }

        /// <summary>
        /// Fonction qui load tout les elements de map 1-3           
        /// </summary>
        private void LoadMap13()
        {
            this.ClearMap();

            Bloc bloc0 = new Bloc(404, 150);
            Bloc bloc1 = new Bloc(404, 175);
            bloc0.BoundsRect = bloc1.BoundsRect = new Rectangle(362, 100, 84, 415);

            Ogre ogre = new Ogre(new Vector2(210, 144));
            this.listeOgres.Add(ogre);
            Ogre ogre1 = new Ogre(new Vector2(360, 144));
            this.listeOgres.Add(ogre1);

            OgreMouvement ogre2 = new OgreMouvement(new Vector2(300, 300));
            ogre2.BoundsRect = new Rectangle(240, 144, 120, 200);
            this.listeOgres.Add(ogre2);

            this.listeBloc.Add(bloc0);
            this.listeBloc.Add(bloc1);
            if (this.boolFood == true)
            {
                Food food = new Food(475, 120);
                this.listeFood.Add(food);
            }
        }

        /// <summary>
        /// Fonction qui load tout les elements de map 1-4           
        /// </summary>
        private void LoadMap14()
        {
            this.ClearMap();

            Ogre ogre = new Ogre(new Vector2(300, 120));
            this.listeOgres.Add(ogre);
            Ogre ogre1 = new Ogre(new Vector2(300, 260));
            this.listeOgres.Add(ogre1);

            if (this.boolClef == false && portesClefOuvert[0] == false)
            {
                Clef clef = new Clef(474, 461);
                this.listeClef.Add(clef);
            }
        }

        /// <summary>
        /// Fonction qui load tout les elements de map 1-5           
        /// </summary>
        private void LoadMap15()
        {
            this.ClearMap();

            Ogre ogre = new Ogre(new Vector2(400, 120));
            this.listeOgres.Add(ogre);
            Ogre ogre1 = new Ogre(new Vector2(400, 480));
            this.listeOgres.Add(ogre1);

            OgreMouvement ogre2 = new OgreMouvement(new Vector2(200, 120));
            this.listeOgres.Add(ogre2);
            ogre2.BoundsRect = new Rectangle(180, 77, 100, 300);

            OgreMouvement ogre3 = new OgreMouvement(new Vector2(200, 480));
            ogre3.BoundsRect = new Rectangle(180, 77, 100, 450);
            this.listeOgres.Add(ogre3);

            Porte porteNord = new Porte(115, 75, Porte.Directions.Nord);
            porteNord.PorteClef = true;
            if (this.portesClefOuvert[0] == false)
            {
                porteNord.Ouvert = false;
            }
            else
            {
                porteNord.Ouvert = true;
            }

            this.listePorte.Add(porteNord);
        }

        /// <summary>
        /// Fonction qui fait S'occupe de switch sur la map         
        /// </summary>
        /// /// <param name="gameTime">Recoie la gameTime.</param>
        private void GestionSwtich(GameTime gameTime)
        {
            foreach (Switch switch1 in this.listeSwitch)
            {
                foreach (Bloc bloc in this.listeBloc)
                {
                    if (switch1.Boutton.Intersects(bloc.AireOccupe))
                    {
                        switch (switch1.Type)
                        {
                            case Switch.Types.Nord:
                                foreach (Porte p in this.listePorte)
                                {
                                    if (p.Direction == Porte.Directions.Nord)
                                    {
                                        p.Ouvert = true;
                                    }

                                    return;
                                }

                                foreach (PorteHorizontale p in this.listePorteHorizontale)
                                {
                                    if (p.Direction == PorteHorizontale.Directions.Nord)
                                    {
                                        p.Ouvert = true;
                                    }

                                    return;
                                }

                                break;
                            case Switch.Types.Est:
                                foreach (Porte p in this.listePorte)
                                {
                                    if (p.Direction == Porte.Directions.Est)
                                    {
                                        p.Ouvert = true;
                                    }
                                }

                                foreach (PorteHorizontale p in this.listePorteHorizontale)
                                {
                                    if (p.Direction == PorteHorizontale.Directions.Est)
                                    {
                                        p.Ouvert = true;
                                    }

                                    return;
                                }

                                break;
                            case Switch.Types.Sud:
                                foreach (Porte p in this.listePorte)
                                {
                                    if (p.Direction == Porte.Directions.Sud)
                                    {
                                        p.Ouvert = true;
                                    }

                                    return;
                                }

                                foreach (PorteHorizontale p in this.listePorteHorizontale)
                                {
                                    if (p.Direction == PorteHorizontale.Directions.Sud)
                                    {
                                        p.Ouvert = true;
                                    }

                                    return;
                                }

                                break;
                            default:
                                foreach (Porte p in this.listePorte)
                                {
                                    if (p.Direction == Porte.Directions.Ouest)
                                    {
                                        p.Ouvert = true;
                                    }

                                    return;
                                }

                                foreach (PorteHorizontale p in this.listePorteHorizontale)
                                {
                                    if (p.Direction == PorteHorizontale.Directions.Ouest)
                                    {
                                        p.Ouvert = true;
                                    }

                                    return;
                                }

                                break;
                        } // fin de switch
                    }
                }

                if (switch1.Boutton.Intersects(new Rectangle((int)this.joueur.Position.X - 5, (int)this.joueur.Position.Y + 10, 10, 10)))
                {
                    switch (switch1.Type)
                    {
                        case Switch.Types.Nord:
                            foreach (Porte p in this.listePorte)
                            {
                                if (p.Direction == Porte.Directions.Nord)
                                {
                                    p.Ouvert = true;
                                }
                            }

                            foreach (PorteHorizontale p in this.listePorteHorizontale)
                            {
                                if (p.Direction == PorteHorizontale.Directions.Nord)
                                {
                                    p.Ouvert = true;
                                }
                            }

                            break;
                        case Switch.Types.Est:
                            foreach (Porte p in this.listePorte)
                            {
                                if (p.Direction == Porte.Directions.Est)
                                {
                                    p.Ouvert = true;
                                }
                            }

                            foreach (PorteHorizontale p in this.listePorteHorizontale)
                            {
                                if (p.Direction == PorteHorizontale.Directions.Est)
                                {
                                    p.Ouvert = true;
                                }
                            }

                            break;
                        case Switch.Types.Sud:
                            foreach (Porte p in this.listePorte)
                            {
                                if (p.Direction == Porte.Directions.Sud)
                                {
                                    p.Ouvert = true;
                                }
                            }

                            foreach (PorteHorizontale p in this.listePorteHorizontale)
                            {
                                if (p.Direction == PorteHorizontale.Directions.Sud)
                                {
                                    p.Ouvert = true;
                                }
                            }

                            break;
                        default:
                            foreach (Porte p in this.listePorte)
                            {
                                if (p.Direction == Porte.Directions.Ouest)
                                {
                                    p.Ouvert = true;
                                }
                            }

                            foreach (PorteHorizontale p in this.listePorteHorizontale)
                            {
                                if (p.Direction == PorteHorizontale.Directions.Ouest)
                                {
                                    p.Ouvert = true;
                                }
                            }

                            break;
                    }
                }
                else
                {
                    switch (switch1.Type)
                    {
                        case Switch.Types.Nord:
                            foreach (Porte p in this.listePorte)
                            {
                                if (p.Direction == Porte.Directions.Nord)
                                {
                                    p.Ouvert = false;
                                }
                            }

                            foreach (PorteHorizontale p in this.listePorteHorizontale)
                            {
                                if (p.Direction == PorteHorizontale.Directions.Nord)
                                {
                                    p.Ouvert = false;
                                }
                            }

                            break;
                        case Switch.Types.Est:
                            foreach (Porte p in this.listePorte)
                            {
                                if (p.Direction == Porte.Directions.Est)
                                {
                                    p.Ouvert = false;
                                }
                            }

                            foreach (PorteHorizontale p in this.listePorteHorizontale)
                            {
                                if (p.Direction == PorteHorizontale.Directions.Est)
                                {
                                    p.Ouvert = false;
                                }
                            }

                            break;
                        case Switch.Types.Sud:
                            foreach (Porte p in this.listePorte)
                            {
                                if (p.Direction == Porte.Directions.Sud)
                                {
                                    p.Ouvert = false;
                                }
                            }

                            foreach (PorteHorizontale p in this.listePorteHorizontale)
                            {
                                if (p.Direction == PorteHorizontale.Directions.Sud)
                                {
                                    p.Ouvert = false;
                                }
                            }

                            break;
                        default:
                            foreach (Porte p in this.listePorte)
                            {
                                if (p.Direction == Porte.Directions.Ouest)
                                {
                                    p.Ouvert = false;
                                }
                            }

                            foreach (PorteHorizontale p in this.listePorteHorizontale)
                            {
                                if (p.Direction == PorteHorizontale.Directions.Ouest)
                                {
                                    p.Ouvert = false;
                                }
                            }

                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Verifie que la position finale du bloc est valide. (Il ne rentre pas dans une mûr, porte, ou autre bloc.)
        /// </summary>
        /// <param name="bloc">Le bloc à tester.</param>
        /// <returns>Return false pour valider le deplacement du joueur</returns>
        private bool ValiderDeplacementBloc(Bloc bloc)
        {
            if (this.joueur.Direction == Personnage.Directions.NordEst ||
                this.joueur.Direction == Personnage.Directions.NordOuest ||
                this.joueur.Direction == Personnage.Directions.SudEst ||
                this.joueur.Direction == Personnage.Directions.SudOuest)
            {
                return false;
            }

            Vector2 destV = bloc.Position;
            switch (this.joueur.Direction)
            {
                case Personnage.Directions.Nord:
                    destV.Y -= 28;
                    break;
                case Personnage.Directions.Est:
                    destV.X += 28;
                    break;
                case Personnage.Directions.Sud:
                    destV.Y += 28;
                    break;
                case Personnage.Directions.Ouest:
                    destV.X -= 28;
                    break;
                default: // Si le joueur se déplace sur un angle, il ne peut pas déplacer le bloc.
                    return false;
            }
            
            if ((this.monde.CouleurDeCollision(destV) != Color.White && 
                this.monde.CouleurDeCollision(destV) != Color.Blue) ||
                destV.Y < 95 ||
                destV.Y > 505 ||
                destV.X < 95 ||
                destV.X > 505) 
            { 
                return false; 
            }

            Rectangle destBloc = new Rectangle((int)destV.X - 14, (int)destV.Y - 14, 28, 28);

            foreach (Bloc bloc1 in this.listeBloc)
            {
                if (destBloc.Intersects(bloc1.AireOccupe))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Fonction qui fait la gestion des projectiles         
        /// </summary>
        /// <param name="gameTime">Game time</param>
        private void GestionProjectile(GameTime gameTime)
        {
            this.JoueurTirerProjectile();

            this.ProjectileReflection();

            this.ProjectilleRebondissement();

            foreach (Projectile pj in this.listeProjectiles)
            {
                if (pj.VideDeProjectile <= 0)
                {
                    this.listeProjectileFini.Add(pj);
                }

                foreach (Ennemi ogre in this.listeOgres)
                {
                    if (pj.CollisionRapide(ogre) && pj.TypeProjectile == Projectile.TypesProjectiles.Joueur)
                    {
                        // Créer un nouvel effet visuel pour l'explosion.
                        this.CreerExplosion(ogre, gameTime);
                        this.listeProjectileFini.Add(pj);
                        this.listeOgresFini.Add(ogre);
                    }
                }

                if (pj.TypeProjectile == Projectile.TypesProjectiles.Ennemi)
                {
                    foreach (Projectile pj1 in this.listeProjectiles)
                    {
                        if (pj1.TypeProjectile == Projectile.TypesProjectiles.Joueur)
                        {
                            if (pj.CollisionRapide(pj1))
                            {
                                // Créer un nouvel effet visuel pour l'explosion.
                                this.CreerExplosion(pj, gameTime);
                                this.listeProjectileFini.Add(pj);
                                this.listeProjectileFini.Add(pj1);
                            }
                        }
                    }
                }

                if (pj.CollisionRapide(this.joueur) && pj.TypeProjectile == Projectile.TypesProjectiles.Ennemi && this.joueur.CouleurCollison > 99)
                {
                    this.joueur.VieDeJoueur--;
                    this.joueur.CouleurCollison = 0;
                    this.bruitageFrapper.Play();

                    // Créer un nouvel effet visuel pour l'explosion.
                    this.CreerExplosion(pj, gameTime);
                    this.listeProjectileFini.Add(pj);
                }
            }

            // Se débarasser des projectile ayant quitté l'écran.
            foreach (Projectile pj in this.listeProjectileFini)
            {
                this.listeProjectiles.Remove(pj);
            }
        }

        /// <summary>
        /// Fonction qui fait tirer les projectiles         
        /// </summary>
        private void JoueurTirerProjectile()
        {
            if (ServiceHelper.Get<IInputService>().TirerNord(1))
            {
                Projectile pj = new Projectile(new Vector2(this.joueur.Position.X, this.joueur.Position.Y), 0);
                pj.TypeProjectile = Projectile.TypesProjectiles.Joueur;

                if (ServiceHelper.Get<IInputService>().DeplacementDroite(0) > 0)
                {
                    pj.VitesseHorizontale += this.joueur.VitesseHorizontal * 1.2f;
                }
                else if (ServiceHelper.Get<IInputService>().DeplacementGauche(0) > 0)
                {
                    pj.VitesseHorizontale -= this.joueur.VitesseHorizontal * 1.2f;
                }

                this.listeProjectiles.Add(pj);
            }
            else if (ServiceHelper.Get<IInputService>().TirerEst(1))
            {
                Projectile pj = new Projectile(new Vector2(this.joueur.Position.X, this.joueur.Position.Y), 2);
                pj.TypeProjectile = Projectile.TypesProjectiles.Joueur;

                if (ServiceHelper.Get<IInputService>().DeplacementAvant(0) > 0)
                {
                    pj.VitesseVerticale -= this.joueur.VitesseVerticale * 1.2f;
                }
                else if (ServiceHelper.Get<IInputService>().DeplacementArriere(0) > 0)
                {
                    pj.VitesseVerticale += this.joueur.VitesseVerticale * 1.2f;
                }

                this.listeProjectiles.Add(pj);
            }
            else if (ServiceHelper.Get<IInputService>().TirerSud(1))
            {
                Projectile pj = new Projectile(new Vector2(this.joueur.Position.X, this.joueur.Position.Y), 4);
                pj.TypeProjectile = Projectile.TypesProjectiles.Joueur;

                if (ServiceHelper.Get<IInputService>().DeplacementDroite(0) > 0)
                {
                    pj.VitesseHorizontale += this.joueur.VitesseHorizontal * 1.2f;
                }
                else if (ServiceHelper.Get<IInputService>().DeplacementGauche(0) > 0)
                {
                    pj.VitesseHorizontale -= this.joueur.VitesseHorizontal * 1.2f;
                }

                this.listeProjectiles.Add(pj);
            }
            else if (ServiceHelper.Get<IInputService>().TirerOuest(1))
            {
                Projectile pj = new Projectile(new Vector2(this.joueur.Position.X, this.joueur.Position.Y), 6);
                pj.TypeProjectile = Projectile.TypesProjectiles.Joueur;

                if (ServiceHelper.Get<IInputService>().DeplacementAvant(0) > 0)
                {
                    pj.VitesseVerticale -= this.joueur.VitesseVerticale * 1.2f;
                }
                else if (ServiceHelper.Get<IInputService>().DeplacementArriere(0) > 0)
                {
                    pj.VitesseVerticale += this.joueur.VitesseVerticale * 1.2f;
                }

                this.listeProjectiles.Add(pj);
            }
        }

        /// <summary>
        /// Fonction qui fait la gestion du rebondissement des projectiles         
        /// </summary>
        private void ProjectileReflection()
        {
            foreach (Projectile pj in this.listeProjectiles)
            {
                // La couleur "rouge" dans les images de collision, a finalement un tinte rouge de 237.
                if (this.monde.CouleurDeCollision(pj.Position).R == 237)
                {
                    if (pj.VitesseHorizontale < 0)
                    {
                        pj.VitesseHorizontale = 1;
                    }
                    else if (pj.VitesseHorizontale > 0)
                    {
                        pj.VitesseHorizontale = -1;
                    }
                }
                else if (this.monde.CouleurDeCollision(pj.Position) == Color.Black)
                {
                    if (pj.VitesseVerticale < 0)
                    {
                        pj.VitesseVerticale = 1;
                    }
                    else if (pj.VitesseVerticale > 0)
                    {
                        pj.VitesseVerticale = -1;
                    }
                }
            }
        }

        /// <summary>
        /// Fonction qui fait la gestion du rebondissement des projectiles sur les blocs         
        /// </summary>
        private void GestionBloc()
        {
            if (this.maBloc.BlockMouvement == true)
            {
                if (this.joueur.Direction == Personnage.Directions.Nord)
                {
                    this.maBloc.VitesseVerticale = -0.1f;
                }
                else if (this.joueur.Direction == Personnage.Directions.Sud)
                {
                    this.maBloc.VitesseVerticale = 0.1f;
                }
                else if (this.joueur.Direction == Personnage.Directions.Est)
                {
                    this.maBloc.VitesseHorizontale = 0.1f;
                }
                else if (this.joueur.Direction == Personnage.Directions.Ouest)
                {
                    this.maBloc.VitesseHorizontale = -0.1f;
                }

                this.bruitageblock.Play();

                this.maBloc.BlockMouvement = false;
            }
        }

        /// <summary>
        /// Fonction qui fait la gestion du rebondissement des projectiles sur les blocs         
        /// </summary>
        private void ProjectilleRebondissement()
        {
            foreach (Projectile pj in this.listeProjectiles)
            {
                foreach (Bloc bloc in this.listeBloc)
                {
                    if (bloc.CollisionBloc(pj))
                    {
                        if (pj.VitesseHorizontale < 0)
                        {
                            pj.VitesseHorizontale = 1;
                        }
                        else if (pj.VitesseHorizontale > 0)
                        {
                            pj.VitesseHorizontale = -1;
                        }

                        if (pj.VitesseVerticale < 0)
                        {
                            pj.VitesseVerticale = 1;
                        }
                        else if (pj.VitesseVerticale > 0)
                        {
                            pj.VitesseVerticale = -1;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Fonction permettant de simuler une explosion de l'astéroïde donné. La fonction
        /// crée un ensemble de particules d'explosition (de 10 à 20 instances de ParticuleExplosion)
        /// positionnées au centre de l'astéroïde, et les ajoute à sa liste de particules à
        /// gérer (attribut privListeParticulesExplosions).
        /// </summary>        
        private void UpdateVieDeJoueur()
        {
            int compteur = this.joueur.VieDeJoueur;
            int distance = 15;
            int distance1 = 15;

            foreach (VieDeJoueur vie in this.listeVideJoueur)
            {
                this.listeVideJoueurFini.Add(vie);
            }

            foreach (VieDeJoueur vie in this.listeVideJoueurFini)
            {
                this.listeVideJoueur.Remove(vie);
            }

            for (; compteur > 0; compteur--)
            {
                if (compteur > 5 && compteur < 11)
                {
                    VieDeJoueur vie = new VieDeJoueur(distance, 45);
                    this.listeVideJoueur.Add(vie);
                    distance += 30;
                }
                else if (compteur > 0)
                {
                    VieDeJoueur vie = new VieDeJoueur(distance1, 15);
                    this.listeVideJoueur.Add(vie);
                    distance1 += 30;
                }
            }
        }

        /// <summary>
        /// Fonction permettant de simuler une explosion de l'astéroïde donné. La fonction
        /// crée un ensemble de particules d'explosition (de 10 à 20 instances de ParticuleExplosion)
        /// positionnées au centre de l'astéroïde, et les ajoute à sa liste de particules à
        /// gérer (attribut privListeParticulesExplosions).
        /// </summary>
        /// <param name="sprite">Astéroïde à faire exploser.</param>
        /// <param name="gameTime">Lecture du temps de jeu écoulé.</param>
        private void CreerExplosion(Sprite sprite, GameTime gameTime)                                                                               ////// explosion
        {
            // Déterminer au hasard le nombre de particules pour représenter l'explosion
            int nombreDeParticules = 10 + this.randomExplosions.Next(11);   // entre 10 et 20 particules

            // Créer les particules et les ajouter à la liste de particules d'explosions à gérer
            for (int i = 0; i < nombreDeParticules; i++)
            {
                ParticuleExplosion particule = new ParticuleExplosion(
                    sprite.Position,                              // positionné au départ sur l'astéroïde
                    this.explosionParticule,                         // texture à utiliser
                    0);      // vitesse de déplacement vertical

                this.listeParticulesExplosions.Add(particule);
            }
        }
    }
}
