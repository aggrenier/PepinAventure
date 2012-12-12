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

        private Boolean bruitageFinOn = false;

        /// <summary>
        /// Effet sonore contenant le bruitage de fin du jeu.
        /// </summary>
        private static SoundEffect bruitageFin;

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
        /// Fond d'écran d'accueil.
        /// </summary>
        private Texture2D ecranAccueil;

        /// <summary>
        /// Fond d'écran de générique.
        /// </summary>
        private Texture2D ecranGenerique;

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
                if (this.monde is map_1_1)
                {
                    return Mondes.MAP_1_1;
                }
                else if (this.monde is map_1_2)
                {
                    return Mondes.MAP_1_2;
                }
                else if (this.monde is map_1_3)
                {
                    return Mondes.MAP_1_3;
                }
                else if (this.monde is map_1_4)
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
                    this.monde = new map_1_1();
                }
                else if (value == Mondes.MAP_1_2)
                {
                    this.monde = new map_1_2();
                }
                else if (value == Mondes.MAP_1_3)
                {
                    this.monde = new map_1_3();
                }
                else if (value == Mondes.MAP_1_4)
                {
                    this.monde = new map_1_4();
                }
                else if (value == Mondes.MAP_1_5)
                {
                    this.monde = new map_1_5();
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
            foreach (Bloc bloc in listeBloc)
            {
                if (new Rectangle((int)this.joueur.PositionPourCollisions.X,
                    (int)this.joueur.PositionPourCollisions.Y,1,1).Intersects(bloc.AireOccupe))
                {
                    return 1.0f;
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
                else if (this.CalculerResistanceAuMouvement(dest) == .9f)                             ///////////******
                    this.joueur.Etat = Personnage.Etats.Tombe;
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
                else if (this.CalculerResistanceAuMouvement(dest) == .9f)                             ///////////******
                    this.joueur.Etat = Personnage.Etats.Tombe;
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
                else if (this.CalculerResistanceAuMouvement(dest) == .9f)                             ///////////******
                    this.joueur.Etat = Personnage.Etats.Tombe;
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

            this.listeOgres = new List<Ennemi>();                                                                                     /////////////////////////// OGRES
            this.listeOgresFini = new List<Ennemi>();

            this.listeSwitch = new List<Sprite>();
            this.listeSwitchFini = new List<Sprite>();

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
            map_1_1.LoadContent(Content, this.graphics);
            map_1_2.LoadContent(Content, this.graphics);
            map_1_3.LoadContent(Content, this.graphics);
            map_1_4.LoadContent(Content, this.graphics);
            map_1_5.LoadContent(Content, this.graphics);

            // Configurer la caméra.
            //this.camera.MondeRect = new Rectangle(0, 0, this.monde.Largeur, this.monde.Hauteur);

            // Charger le sprite représentant le joueur.
            Joueur.LoadContent(Content, this.graphics);

            // Créer et initialiser le sprite du joueur.
            this.joueur = new Joueur(1150, 300);
            this.joueur.BoundsRect = new Rectangle(0, 0, 600, 600);

            // Imposer la palette de collisions au déplacement du joueur.
            this.joueur.GetValiderDeplacement = this.ValiderDeplacement; 

            // Construire une grille de tuiles franchissables pour le pathfinding des ogres (ils peuvent se déplacer
            // uniquement sur terre).
            PFGrille grille = null;

            Projectile.LoadContent(Content, this.graphics);

            Bloc.LoadContent(Content, this.graphics);

            Ogre.LoadContent(Content, this.graphics);
            OgreMouvement.LoadContent(Content, this.graphics);

            Switch.LoadContent(Content, this.graphics);

            // Charger les textures associées aux effets visuels gérées par Game.
            this.explosionParticule = Content.Load<Texture2D>("Textures\\Effets\\explosion");

            this.MondeCourant = Mondes.MAP_1_1;
            LoadMap11();

            // Imposer la palette de collisions au déplacement du joueur.                       ///////////******
            this.joueur.GetResistanceAuMouvement = this.CalculerResistanceAuMouvement;          ///////////******

            // Charger le bruitage de fond du jeu.
            bruitageFond = Content.Load<SoundEffect>("Audio\\Musique\\zelda_3");

            // Charger le bruitage de fond du jeu.
            bruitageFin = Content.Load<SoundEffect>("Audio\\Musique\\zelda_Fin");

            // Charger le bruitage de fond du jeu.
            bruitageblock = Content.Load<SoundEffect>("Audio\\Effets\\Bloc\\bloc");///////////////////

            // Sélectionner et paramétrer le bruitage de fond.
            this.bruitageFondActif = bruitageFond.CreateInstance();
            this.bruitageFondActif.Volume = 0.80f;
            this.bruitageFondActif.IsLooped = true;

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
                    this.EtatJeu = Etats.Jouer;
                // Rien d'autre à faire alors on quitte la fonction 
                base.Update(gameTime);
                return;
            }

            // Si le jeu est en cours de terminaison, l'écran de générique demeure affichée
            // jusqu'à ce que le générique soit terminé ou que l'usager presse la touche
            // appropriée
            if (this.EtatJeu == Etats.Quitter)
            {

                if (bruitageFinOn != true)
                {
                    bruitageFin.Play();
                    bruitageFinOn = true;
                }
                // L'usager veut-il quitter immédiatement
                if (ServiceHelper.Get<IInputService>().Quitter(0))
                    this.Exit();
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
                this.bruitageFondActif.Volume = 0.1f;                                            //////////////////////////////
            }

            // Mettre à jour le sprite du joueur puis centrer la camera sur celui-ci.
            this.joueur.Update(gameTime, this.graphics);

            if (this.joueur.Etat == Personnage.Etats.Tombe && this.joueur.ContTombe > 60)
            {
                this.joueur.AngleRotation = this.joueur.ContTombe = 0;
                this.joueur.Etat = Personnage.Etats.Stationnaire;
                this.joueur.Echelle = 1.0f;
                this.joueur.Position = monde.PositionInitiale;
            }


            this.camera.Centrer(this.joueur.Position);
            // Se débarasser des astéroïdes ayant quitté l'écran.
            foreach (Ennemi ogre in listeOgresFini)
            {
                this.listeOgres.Remove(ogre);
            }

            // Mettre à jour les ogres.
            foreach (Ennemi ogre in this.listeOgres)
            {
                //ogre.GrillePathFinding.Destination = this.joueur.Position;
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
                        Projectile pj = new Projectile(new Vector2(ogre.Position.X +60, ogre.Position.Y - 60), 1);
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


                ogre.Update(gameTime, this.graphics);
            }

            GestionProjectile(gameTime);

            GestionBloc();

            // Mettre à jour les particules d'explosion
            this.UpdateParticulesExplosions(gameTime); 

            foreach (Projectile pj in this.listeProjectiles)
            {
                pj.Update(gameTime, this.graphics);

            }

            foreach (Bloc bloc in listeBloc)
            {
                if (bloc.VideDeBloc > 0)
                {
                    bloc.Update(gameTime, this.graphics);
                }
            }


            foreach (Bloc bloc in this.listeBloc)
            {
                listeBlocFini.Add(bloc);
            }           

            // Vérifier si le joueur a atteint une sortie du monde.
            if (this.monde.AtteintUneSortie(this.joueur))
            {
                if (this.MondeCourant == Mondes.MAP_1_1)
                {
                    this.MondeCourant = Mondes.MAP_1_2;
                    this.joueur.Position = new Vector2(300, 490);

                    LoadMap12();

                }
                else if (this.MondeCourant == Mondes.MAP_1_2)
                {

                    if (this.joueur.Position.Y > 500)
                    {
                        this.MondeCourant = Mondes.MAP_1_1;
                        this.joueur.Position = new Vector2(300, 60);

                        LoadMap11();
                    }
                    else if (this.joueur.Position.Y < 60)
                    {
                        this.MondeCourant = Mondes.MAP_1_3;
                        this.joueur.Position = this.monde.PositionInitiale;

                        LoadMap13();
                    }

                    else if (this.joueur.Position.X > 510)
                    {
                        this.MondeCourant = Mondes.MAP_1_4;
                        this.joueur.Position = new Vector2(97, 300);

                        LoadMap14();

                    }
                    else if (this.joueur.Position.X < 85)
                    {
                        this.MondeCourant = Mondes.MAP_1_5;
                        this.joueur.Position = new Vector2(500, 285);

                        LoadMap15();
                    }
                }

                else if (this.MondeCourant == Mondes.MAP_1_3)
                {
                    this.MondeCourant = Mondes.MAP_1_2;
                    this.joueur.Position = new Vector2(300, 60);

                    LoadMap12();
                }

                else if (this.MondeCourant == Mondes.MAP_1_4)
                {


                    this.MondeCourant = Mondes.MAP_1_2;
                    this.joueur.Position = new Vector2(500, 280);


                    LoadMap12();

                }
                else if (this.MondeCourant == Mondes.MAP_1_5)
                {
                    if (this.joueur.Position.X > 510)
                    {
                        this.MondeCourant = Mondes.MAP_1_2;
                        this.joueur.Position = new Vector2(90, 290);
                        LoadMap12();
                    }
                    else if (this.joueur.Position.Y < 60)
                    {
                        this.EtatJeu = Etats.Quitter;
                    }

                }
            }

            Console.WriteLine(joueur.Position);

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

            //Suspendre au besoin les effets sonores des ogres.
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
        protected void UpdateParticulesExplosions(GameTime gameTime)                                                                                    //explosion
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

            //Ajouter les ogres à afficher à la liste.
            //listeDraw.Add(this.joueur);
            foreach (Ennemi ogre in this.listeOgres)
            {
                listeDraw.Add(ogre);
            }

            // Afficher les blocs.
            foreach (Bloc bloc in this.listeBloc)
            {
                listeDraw.Add(bloc);
            }
            foreach (Switch switcht1 in this.listeSwitch)
            {
                listeDraw.Add(switcht1);
            }

            // Afficher les projectiles.
            foreach (Projectile pj in this.listeProjectiles)
            {
                if (pj.TypeProjectile == Projectile.TypesProjectiles.Joueur)
                {
                    spriteBatch.Draw(
                        pj.Texture,                 // texture
                        pj.Position,                // position
                        null,                       // sourceRectangle
                        Color.Chartreuse,                // couleur
                        0,  // angle de rotation
                        new Vector2(16, 16),   
                        .4f + pj.VideDeProjectile % 1.001f,             // échelle d'affichage
                        SpriteEffects.None,         // effets
                        0.0f);                      // profondeur de couche (layer depth));
                }
                else
                {
                    spriteBatch.Draw(
                        pj.Texture,                 // texture
                        pj.Position,                // position
                        null,                       // sourceRectangle
                        Color.LightSalmon,                // couleur
                        0,                          // angle de rotation
                        new Vector2(16, 16),  
                        pj.VideDeProjectile % 1.1f,             // échelle d'affichage
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

            // Afficher le sprite du joueur.
            if (this.joueur.Etat != Personnage.Etats.Tombe)
                this.joueur.Draw(this.camera, this.spriteBatch);
            else
            {
                // Afficher le joueur en etat de tombe
                spriteBatch.Draw(
                    joueur.Texture,             // texture
                    joueur.Position,            // position
                    null,                       // sourceRectangle
                    Color.White,                // couleur
                    this.joueur.AngleRotation,  // angle de rotation
                    new Vector2(16, 16),        // origine de rotation
                    joueur.Echelle,             // échelle d'affichage
                    SpriteEffects.None,         // effets
                    0.0f);                      // profondeur de couche (layer depth)
            }

            // Afficher les messages selon l'état du jeu.
            this.DrawMessages(this.spriteBatch);

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
            foreach (ParticuleExplosion particule in this.listeParticulesExplosions)                                                        //////////Explosion
            {
                particule.Draw(this.spriteBatch);
            }

            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Routine d'affichage de message (centré à l'écran) correspondant à l'état courant du jeu.
        /// </summary>
        /// <param name="spriteBatch">Tampon d'affichage.</param>
        protected void DrawMessages(SpriteBatch spriteBatch)
        {
            string output = string.Empty;      // Message à afficher

            // Déterminer le message à afficher selon l'état du jeu.
            switch (this.EtatJeu)
            {
                case Etats.Pause:
                    if (this.MenuCourant == null)
                    {
                        output = "Pause (pressez P pour continuer...)";
                    }

                    break;

                default:
                    output = string.Empty;
                    break;
            }

            // Afficher le message s'il y a lieu.
            if (output.Length > 0)
            {
                // L'origine du message sera positionnée au centre de l'écran.
                Vector2 centreEcran = new Vector2(
                    this.graphics.GraphicsDevice.Viewport.Width / 2,
                    this.graphics.GraphicsDevice.Viewport.Height / 2);


                int decalageY = 50;
                // Appliquer le décalage vertical par rapport au centre de l'écran.
                centreEcran.Y += decalageY;

                // Splitter le message en fonction des \n dans celui-ci, afin de center chaque chaîne du message
                // à l'écran.
                string[] lignes = output.Split('\n');

                Color couleur = Color.White;

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
        /// Fonction qui parcourt les listes et clear les item pour reinitialiser les cartes            
        /// </summary>
        private void ClearMap()
        {
            foreach (Bloc bloc in this.listeBloc)
            {
                listeBlocFini.Add(bloc);
            }

            foreach (Bloc bloc in listeBlocFini)
            {
                this.listeBloc.Remove(bloc);
            }

            foreach (Projectile pj in this.listeProjectiles)
            {
                listeProjectileFini.Add(pj);
            }
            // Se débarasser des astéroïdes ayant quitté l'écran.
            foreach (Projectile pj in listeProjectileFini)
            {
                this.listeProjectiles.Remove(pj);
            }

            foreach (Ennemi ogre in this.listeOgres)
            {
                listeOgresFini.Add(ogre);
            }
            // Se débarasser des astéroïdes ayant quitté l'écran.
            foreach (Ennemi ogre in listeOgresFini)
            {
                this.listeOgres.Remove(ogre);
            }

            foreach (Switch switch1 in this.listeSwitch)
            {
                listeSwitchFini.Add(switch1);
            }

            foreach (Switch switch1 in this.listeSwitchFini)
            {
                this.listeSwitch.Remove(switch1);
            }

           
        }

        /// <summary>
        /// Fonction qui load tout les elements de map 1-1           
        /// </summary>
        private void LoadMap11()
        {
            ClearMap();

            Bloc bloc0 = new Bloc(300, 133);
            bloc0.BoundsRect = new Rectangle(91, 91, 415, 415);

            this.listeBloc.Add(bloc0);

            OgreMouvement ogre = new OgreMouvement(new Vector2(300, 300));
            this.listeOgres.Add(ogre);
           
        }

        /// <summary>
        /// Fonction qui load tout les elements de map 1-2           
        /// </summary>
        private void LoadMap12()
        {
            ClearMap();

            Bloc bloc0 = new Bloc(106, 340);
            bloc0.BoundsRect = new Rectangle(91, 91, 415, 415);
            
            Bloc bloc1 = new Bloc(133, 300);
            bloc1.BoundsRect = new Rectangle(91, 91, 415, 415);

            Bloc bloc2 = new Bloc(106, 255);
            bloc2.BoundsRect = new Rectangle(91, 91, 415, 415);

            this.listeBloc.Add(bloc0);
            this.listeBloc.Add(bloc1);
            this.listeBloc.Add(bloc2);

            Ogre ogre = new Ogre(new Vector2(152, 144));
            this.listeOgres.Add(ogre);
            Ogre ogre1 = new Ogre(new Vector2(444, 144));
            this.listeOgres.Add(ogre1);

            Switch switch1 = new Switch(133, 230);
            this.listeSwitch.Add(switch1);

            Switch switch2 = new Switch(203, 300);
            this.listeSwitch.Add(switch2);
        }
        /// <summary>
        /// Fonction qui load tout les elements de map 1-3           
        /// </summary>
        private void LoadMap13()
        {
            ClearMap();

            Bloc bloc0 = new Bloc(404, 150);
            Bloc bloc1 = new Bloc(404, 175);

            Ogre ogre = new Ogre(new Vector2(300, 120));
            this.listeOgres.Add(ogre);
            Ogre ogre1 = new Ogre(new Vector2(300, 260));
            this.listeOgres.Add(ogre1);

            this.listeBloc.Add(bloc0);
            this.listeBloc.Add(bloc1);

        }
        /// <summary>
        /// Fonction qui load tout les elements de map 1-4           
        /// </summary>
        private void LoadMap14()
        {
            ClearMap();

            Ogre ogre = new Ogre(new Vector2(300, 120));
            this.listeOgres.Add(ogre);
            Ogre ogre1 = new Ogre(new Vector2(300, 260));
            this.listeOgres.Add(ogre1);
        }
        /// <summary>
        /// Fonction qui load tout les elements de map 1-5           
        /// </summary>
        private void LoadMap15()
        {
            ClearMap();

            Ogre ogre = new Ogre(new Vector2(300, 120));
            this.listeOgres.Add(ogre);
            Ogre ogre1 = new Ogre(new Vector2(300, 260));
            this.listeOgres.Add(ogre1);

        }

        /// <summary>
        /// Fonction qui fait la gestion des projectiles         
        /// </summary>
        private void GestionProjectile(GameTime gameTime)
        {
            if (ServiceHelper.Get<IInputService>().tirerNord(1))
            {
                Projectile pj = new Projectile(new Vector2(joueur.Position.X, this.joueur.Position.Y), 0);
                pj.TypeProjectile = Projectile.TypesProjectiles.Joueur;

                if (ServiceHelper.Get<IInputService>().DeplacementDroite(0) > 0)
                {
                    pj.vitesHorizontale += (this.joueur.VitesseHorizontal * 1.2f);
                }
                else if (ServiceHelper.Get<IInputService>().DeplacementGauche(0) > 0)
                {
                    pj.vitesHorizontale -= (this.joueur.VitesseHorizontal * 1.2f);
                }


                this.listeProjectiles.Add(pj);
            }

            else if (ServiceHelper.Get<IInputService>().tirerEst(1))
            {
                Projectile pj = new Projectile(new Vector2(joueur.Position.X, this.joueur.Position.Y), 2);
                pj.TypeProjectile = Projectile.TypesProjectiles.Joueur;

                if (ServiceHelper.Get<IInputService>().DeplacementAvant(0) > 0)
                {
                    pj.vitesseVerticale -= (this.joueur.VitesseVerticale * 1.2f);
                }
                else if (ServiceHelper.Get<IInputService>().DeplacementArriere(0) > 0)
                {
                    pj.vitesseVerticale += (this.joueur.VitesseVerticale * 1.2f);
                }

                this.listeProjectiles.Add(pj);
            }

            else if (ServiceHelper.Get<IInputService>().tirerSud(1))
            {
                Projectile pj = new Projectile(new Vector2(joueur.Position.X, this.joueur.Position.Y), 4);
                pj.TypeProjectile = Projectile.TypesProjectiles.Joueur;

                if (ServiceHelper.Get<IInputService>().DeplacementDroite(0) > 0)
                {
                    pj.vitesHorizontale += (this.joueur.VitesseHorizontal * 1.2f);
                }
                else if (ServiceHelper.Get<IInputService>().DeplacementGauche(0) > 0)
                {
                    pj.vitesHorizontale -= (this.joueur.VitesseHorizontal * 1.2f);
                }

                this.listeProjectiles.Add(pj);
            }

            else if (ServiceHelper.Get<IInputService>().tirerOuest(1))
            {
                Projectile pj = new Projectile(new Vector2(joueur.Position.X, this.joueur.Position.Y), 6);
                pj.TypeProjectile = Projectile.TypesProjectiles.Joueur;

                if (ServiceHelper.Get<IInputService>().DeplacementAvant(0) > 0)
                {
                    pj.vitesseVerticale -= (this.joueur.VitesseVerticale * 1.2f);
                }
                else if (ServiceHelper.Get<IInputService>().DeplacementArriere(0) > 0)
                {
                    pj.vitesseVerticale += (this.joueur.VitesseVerticale * 1.2f);
                }

                this.listeProjectiles.Add(pj);
            }

            ProjectileReflection();
            ProjectilleRebondissement();

            foreach (Projectile pj in this.listeProjectiles)
            {
                if (pj.VideDeProjectile <= 0)
                {
                    listeProjectileFini.Add(pj);
                }
                foreach (Ennemi ogre in listeOgres)                                                       ////// explosion /////////////
                {
                    if (pj.CollisionRapide(ogre) && pj.TypeProjectile == Projectile.TypesProjectiles.Joueur)
                    {
                        //Créer un nouvel effet visuel pour l'explosion.
                        this.CreerExplosion(ogre, gameTime);
                        listeProjectileFini.Add(pj);
                        listeOgresFini.Add(ogre);
                    }
                }
                if (pj.TypeProjectile == Projectile.TypesProjectiles.Ennemi)
                {
                    foreach (Projectile pj1 in this.listeProjectiles)
                    {
                        if (pj1.TypeProjectile == Projectile.TypesProjectiles.Joueur)
                            if (pj.CollisionRapide(pj1))
                            {
                                // Créer un nouvel effet visuel pour l'explosion.
                                this.CreerExplosion(pj, gameTime);
                                listeProjectileFini.Add(pj);
                                listeProjectileFini.Add(pj1);

                            }
                    }
                }
            }
            // Se débarasser des projectile ayant quitté l'écran.
            foreach (Projectile pj in listeProjectileFini)
            {
                this.listeProjectiles.Remove(pj);
            }
        }

        /// <summary>
        /// Fonction qui fait la gestion du rebondissement des projectiles         
        /// </summary>
        private void ProjectileReflection()
        {
            foreach (Projectile pj in this.listeProjectiles)
            {
                if (this.monde.CouleurDeCollision(pj.Position) == Color.Black)
                {
                    if (pj.vitesHorizontale < 0)
                    {
                        pj.vitesHorizontale = 1;
                    }

                    else if (pj.vitesHorizontale > 0)
                    {
                        pj.vitesHorizontale = -1;
                    }

                    if (pj.vitesseVerticale < 0)
                    {
                        pj.vitesseVerticale = 1;
                    }

                    else if (pj.vitesseVerticale > 0)
                    {
                        pj.vitesseVerticale = -1;
                    }
                }
            }
        }

        /// <summary>
        /// Fonction qui fait la gestion du rebondissement des projectiles sur les blocs         
        /// </summary>
        private void GestionBloc()
        {
            foreach (Bloc bloc in listeBloc)
            {
                if (this.joueur.CollisionBloc(bloc) && ServiceHelper.Get<IInputService>().Sauter(1) && bloc.BlockMouvement == true)
                {
                    if (this.joueur.Direction == Personnage.Directions.Nord)
                    {
                        bloc.vitesseVerticale = -0.1f;
                    }
                    else if (this.joueur.Direction == Personnage.Directions.Sud)
                    {
                        bloc.vitesseVerticale = 0.1f;
                    }
                    else if (this.joueur.Direction == Personnage.Directions.Est)
                    {
                        bloc.vitesseHorizontale = 0.1f;
                    }
                    else if (this.joueur.Direction == Personnage.Directions.Ouest)
                    {
                        bloc.vitesseHorizontale = -0.1f;
                    }

                    bruitageblock.Play();

                    bloc.BlockMouvement = false;
                }
            }
        }

        /// <summary>
        /// Fonction qui fait la gestion du rebondissement des projectiles sur les blocs         
        /// </summary>
        private void ProjectilleRebondissement()
        {
            foreach (Projectile pj in listeProjectiles)
            {
                foreach (Bloc bloc in listeBloc)
                {
                    if (bloc.CollisionBloc(pj))
                    {
                        if (pj.vitesHorizontale < 0)
                        {
                            pj.vitesHorizontale = 1;
                        }

                        else if (pj.vitesHorizontale > 0)
                        {
                            pj.vitesHorizontale = -1;
                        }

                        if (pj.vitesseVerticale < 0)
                        {
                            pj.vitesseVerticale = 1;
                        }

                        else if (pj.vitesseVerticale > 0)
                        {
                            pj.vitesseVerticale = -1;
                        }
                    }
                }
            }
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
        /// Fonction permettant de simuler une explosion de l'astéroïde donné. La fonction
        /// crée un ensemble de particules d'explosition (de 10 à 20 instances de ParticuleExplosion)
        /// positionnées au centre de l'astéroïde, et les ajoute à sa liste de particules à
        /// gérer (attribut privListeParticulesExplosions).
        /// </summary>
        /// <param name="asteriode">Astéroïde à faire exploser.</param>
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
