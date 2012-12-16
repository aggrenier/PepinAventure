//-----------------------------------------------------------------------
// <copyright file="Sprite.cs" company="Marco Lavoie">
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
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Classe abstraite de base des sprites du jeu.
    /// </summary>
    public abstract class Sprite
    {
        /// <summary>
        /// Attribut stockant la position du centre du sprite.
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// Attribut statique contenant le rectangle confinant les mouvements du sprite.
        /// </summary>
        private Rectangle boundsRect;

        /// <summary>
        /// Attribut contrôlant le rayon appliquée au sprite pour la détection approximative de collisions.
        /// </summary>
        private float rayonDeCollision;

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite. On invoque l'autre constructeur.
        /// </summary>
        /// <param name="position">Coordonnées initiales horizontale et verticale du sprite.</param>
        public Sprite(Vector2 position)
            : this(position.X, position.Y) 
        { 
        }

        /// <summary>
        /// Constructeur paramétré recevant la position du sprite.
        /// </summary>
        /// <param name="x">Coordonnée initiale x (horizontale) du sprite.</param>
        /// <param name="y">Coordonnée initiale y (verticale) du sprite.</param>
        public Sprite(float x, float y)
        {
            this.Position = new Vector2(x, y);

            // Aucun rayon, car le sprite n'a peut-être pas encore de texture.
            this.rayonDeCollision = 0.0f;
        }

        /// <summary>
        /// Propriété abstraite pour manipuler la texture du sprite. Doit être
        /// surchangée dans les classes dérivées afin de manipuler une Texture2D.
        /// </summary>
        public abstract Texture2D Texture
        {
            get;
        }

        /// <summary>
        /// Accesseur de l'attribut privé position contrôlant la position centrale du
        /// sprite. Le mutateur s'assure que la position fournie est confinée aux bornes
        /// du monde (en fonction de l'attribut BoundsRect).
        /// </summary>
        public virtual Vector2 Position
        {
            get 
            { 
                return this.position; 
            }

            // Le setter s'assure que la nouvelle position n'excède pas les bornes de mouvements
            // si elles sont fournies.
            set
            {
                this.position = value;

                // Limiter le mouvement si un boundsRect est fourni.
                this.ClampPositionToBoundsRect();
            }
        }

        /// <summary>
        /// Accesseur de l'attribut privé boundsRect contrôlant les bornes de positionnement
        /// du sprite. Le mutateur s'assure que la position courante du sprite est confinée
        /// aux nouvelles bornes du monde.
        /// </summary>
        public virtual Rectangle BoundsRect 
        {
            get 
            { 
                return this.boundsRect; 
            }

            // Le setter s'assurer que la position courante est confinée au nouvelles bornes.
            set
            {
                this.boundsRect = value;
                this.Position = this.position;       // exploiter le setter de _position 
            }
        }

        /// <summary>
        /// Accesseur surchargeable pour obtenir la largeur du sprite.
        /// </summary>
        public virtual int Width
        {
            get { return this.Texture.Width; }
        }

        /// <summary>
        /// Accesseur surchargeable pour obtenir la hauteur du sprite.
        /// </summary>
        public virtual int Height
        {
            get { return this.Texture.Height; }
        }

        /// <summary>
        /// Accesseur pour attribut contrôlant le rayon appliquée au sprite pour la détection 
        /// approximative de collisions.
        /// </summary>
        public virtual float RayonDeCollision
        {
            // Si aucun rayon n'est explicitement fourni, calculer un implicitement
            // de façon à inclure la totalité de la texture.
            get 
            {
                if (this.rayonDeCollision > 0.0f)
                {
                    return this.rayonDeCollision;
                }
                else
                {
                    return (float)Math.Sqrt((this.Width * this.Width) + (this.Height * this.Height)) / 4.0f;
                }
            }

            set 
            { 
                this.rayonDeCollision = value; 
            }
        }

        /// <summary>
        /// Fonction membre abstraite (doit être surchargée) mettant à jour le sprite.
        /// </summary>
        /// <param name="gameTime">Gestionnaire de temps de jeu.</param>
        /// <param name="graphics">Gestionnaire de périphérique d'affichage.</param>
        public abstract void Update(GameTime gameTime, GraphicsDeviceManager graphics);

        /// <summary>
        /// Affiche à l'écran le sprite en fonction de la position de la camera, si une est 
        /// fournie.
        /// </summary>
        /// <param name="camera">Caméra à exploiter pour l'affichage.</param>
        /// <param name="spriteBatch">Gestionnaire d'affichage en batch aux périphériques.</param>
        public virtual void Draw(Camera camera, SpriteBatch spriteBatch)
        {
            // Comme l'attribut _position contient la position centrée du sprite mais
            // que Draw() considère la position fournie comme celle de l'origine du
            // sprite, il faut décaler _position en conséquence avant d'invoquer Draw().
            this.ForcerPosition(this.Position.X - (this.Width / 2), this.Position.Y - (this.Height / 2));

            // Créer destRect aux coordonnées du sprite dans le monde. À noter que
            // les dimensions de destRect sont constantes.
            Rectangle destRect = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);

            // Afficher le sprite s'il est visible.
            if (camera == null)
            {
                // Afficher la texture.
                spriteBatch.Draw(this.Texture, destRect, Color.White);
            }
            else if (camera.EstVisible(destRect))
            {
                // Décaler la destination en fonction de la caméra. Ceci correspond à transformer destRect 
                // de coordonnées logiques (i.e. du monde) à des coordonnées physiques (i.e. de l'écran).
                camera.Monde2Camera(ref destRect);

                // Afficher la texture à l'écran.
                spriteBatch.Draw(this.Texture, destRect, Color.White);
            }

            // Remettre _position au centre du sprite.
            this.ForcerPosition(this.Position.X + (this.Width / 2), this.Position.Y + (this.Height / 2));
        }

        /// <summary>
        /// Affiche à l'écran le sprite. L'affichage est délégué à l'autre surcharge de Draw.
        /// </summary>
        /// <param name="spriteBatch">Gestionnaire d'affichage en batch aux périphériques.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            this.Draw(null, spriteBatch);
        }

        /// <summary>
        /// Fonction extrayant l'information de couleur pour chacun de ses pixels. Ces informations
        /// sont retournées dans un tableau de couleurs créé à cet effet.
        /// </summary>
        /// <returns>Tableau de couleurs, une couleur par pixel.</returns>
        public virtual Color[] ExtraireCouleurs()
        {
            // Extraire les données de couleurs dans un nouveau tableau
            Color[] data = new Color[this.Width * this.Height];
            this.Texture.GetData<Color>(data);

            return data;   // retourner le tableau de couleurs
        }

        /// <summary>
        /// Fonction vérifiant si this est en collision avec un des sprites de la liste fournie en 
        /// paramètre. La routine fait appel à l'autre fonction Collision(Sprite) qui fait la
        /// détection de collision.
        /// </summary>
        /// <param name="cibles">Liste de sprite à vérifier s'il y a collision avec this.</param>
        /// <returns>Un sprite étant en collision avec this; false sinon.</returns>
        public virtual Sprite Collision(List<Sprite> cibles)
        {
            // Vérifier s'il y a collision avec chaque sprite de la liste donnée
            foreach (Sprite sprite in cibles)
            {
                if (this.Collision(sprite))
                {
                    // Il y a collision avec this, alors on retourne le sprite l'ayant occasionné.
                    return sprite;
                }
            }

            return null;    // aucune collision détectée
        }

        /// <summary>
        /// Fonction vérifiant si this est en collision avec le sprite fourni en paramètre. La routine
        /// effectue deux tests : le premier, rapide, consiste à effectuer une détection de collision
        /// par forme englobante (i.e. un cercle). Si ce premier test indique un potentiel de collision,
        /// un second test plus précis, la détection de collisions par superposition de pixels, est
        /// appliqué.
        /// </summary>
        /// <param name="cible">Sprite à vérifier s'il y a collision avec this.</param>
        /// <returns>Vrai si this est en collision avec cible.</returns>
        public virtual bool Collision(Sprite cible)
        {
            // Appliquer premièrement la détection par forme englobante
            float distance = (float)Math.Sqrt(Math.Pow(this.Position.X - cible.Position.X, 2f) + Math.Pow(this.Position.Y - cible.Position.Y, 2f));
            if (distance > (this.RayonDeCollision + cible.RayonDeCollision))
            {
                return false;
            }

            // Il y a risque de collision, donc on applique la détection par superposition de pixels
            // Premièrement, déterminer le rectangle de coordonnées pour chaque sprite (ne pas oublié 
            // que le sprite est centré à Position, donc il faut compenser pour son origine)
            Rectangle rectangleA = new Rectangle(
                                        (int)(this.Position.X - (this.Width / 2)),
                                        (int)(this.Position.Y - (this.Height / 2)),
                                        this.Width,
                                        this.Height);
            Rectangle rectangleB = new Rectangle(
                                        (int)(cible.Position.X - (cible.Width / 2)),
                                        (int)(cible.Position.Y - (cible.Height / 2)),
                                        cible.Width,
                                        cible.Height);

            // Obtenir les données de couleur pour chaque sprite
            Color[] dataA = this.ExtraireCouleurs();
            Color[] dataB = cible.ExtraireCouleurs();

            // Déterminer les coordonnées du rectangle résultant de l'intersection des deux sprite
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Comparer chaque pixel dans le rectangle d'intersection : si deux pixels correspondants ne sont
            // pas transparents, alors il y a collision
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Identifier la couleur du pixel de chaque sprite
                    Color colorA = dataA[(x - rectangleA.Left) + ((y - rectangleA.Top) * rectangleA.Width)];
                    Color colorB = dataB[(x - rectangleB.Left) + ((y - rectangleB.Top) * rectangleB.Width)];

                    // Effectuer un ET logique des deux couleurs
                    if ((colorA.A & colorB.A) != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Fonctione de Collision Rapide, sans superposition de pixel       
        /// </summary>
        /// <param name="cible">Sprite à vérifier s'il y a collision avec this.</param>
        /// <returns>Vrai si this est en collision avec cible.</returns>
        public virtual bool CollisionRapide(Sprite cible)
        {
            // Appliquer premièrement la détection par forme englobante
            float distance = (float)Math.Sqrt(Math.Pow(this.Position.X - cible.Position.X, 2f) + Math.Pow(this.Position.Y - cible.Position.Y, 2f));
            if (distance < (this.RayonDeCollision + cible.RayonDeCollision))
            {               
                return true;
            }

            return false;
        }

        /// <summary>
        /// Fonctione de Collision Rapide avec les blocs, sans superposition de pixel       
        /// </summary>
        /// <param name="cible">Sprite à vérifier s'il y a collision avec this.</param>
        /// <returns>Vrai si this est en collision avec cible.</returns>
        public virtual bool CollisionBloc(Sprite cible)
        {
            Rectangle rectSprite = new Rectangle((int)this.position.X - 5, (int)this.position.Y - 5, 10, 10);
            Rectangle rectbloc = new Rectangle((int)cible.Position.X - 14, (int)cible.Position.Y - 14, 28, 28);

            if (rectSprite.Intersects(rectbloc))
            {
                Console.WriteLine(this.RayonDeCollision);
                return true;
            }

            return false;
        }

        /// <summary>Fonction restreignant _position à l'intérieur des limites fournies par boundsRect si
        /// de telles limites sont fournies.
        /// </summary>
        protected virtual void ClampPositionToBoundsRect()
        {
            // Limiter le mouvement si un boundsRect est fourni.
            if (!this.boundsRect.IsEmpty)
            {
                // On divise la taille du sprite par 2 car _position indique le centre du sprite.
                this.position.X = MathHelper.Clamp(this.position.X, this.BoundsRect.Left + (this.Width / 2), this.BoundsRect.Right - (this.Width / 2));
                this.position.Y = MathHelper.Clamp(this.position.Y, this.BoundsRect.Top + (this.Height / 2), this.BoundsRect.Bottom - (this.Height / 2));
            }
        }

        /// <summary>
        /// Cette fonction permet de forcer la position du sprite sans égard aux bornes
        /// de confinement du déplacement (i.e. sans égard à BoundsRect). Cette fonction
        /// membre est exploitée par cetraines classes dérivées pour contourner
        /// l'accesseur de l'attribut privé position.
        /// </summary>
        /// <param name="x">Coordonnée x de la position du sprite du joueur.</param>
        /// <param name="y">Coordonnée y de la position du sprite du joueur.</param>
        protected void ForcerPosition(float x, float y)
        {
            this.position.X = x;
            this.position.Y = y;
        }
    }
}