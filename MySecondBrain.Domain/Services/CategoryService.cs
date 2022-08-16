using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Web;

namespace MySecondBrain.Domain.Services
{
    public class CategoryService
    {

        /// <summary>
        /// Renvoie tous les dossier d'un user. 
        /// </summary>
        /// <returns>Liste des dossiers</returns>
        public static List<Infrastructure.DB.Dossier> GetAllDossiers(ClaimsPrincipal user)
        {
            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {
                return db.Dossiers.Where(n => n.User.Email == user.Identity.Name).ToList();

            }
            
        }

        /// <summary>
        /// Renvoie le nom de dossiers d'un user. 
        /// </summary>
        /// <returns>Nom du dossier</returns>
        public static string GetDossierName(int dossierId)
        {
            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {
                var dossier = db.Dossiers.Where(n => n.Iddossier == dossierId).SingleOrDefault();
                return dossier.Nom;

            }

        }

        /// <summary>
        /// Renvoie une category grace a son id
        /// </summary>
        /// <returns>Category</returns>
        public static Infrastructure.DB.Dossier GetCategory(int catId)
        {
            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {
                return db.Dossiers.Find(catId);
            }
        }

        /// <summary>
        /// Créer une categorie
        /// </summary>
        /// <param name="category">La Categorie/Dossier à créer</param>
        public static void CreateCategory(Infrastructure.DB.Dossier dossier)
        {
            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {
                db.Dossiers.Add(dossier);
                db.SaveChanges();

            }
        }

        /// <summary>
        /// modifie le nom d'une categorie
        /// </summary>
        /// <param name="category">Le nom</param>
        public static void EditCategory(Infrastructure.DB.Dossier dossier)
        {
            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {

                db.Dossiers.Update(dossier);
                db.SaveChanges();
              
            }
        }

        /// <summary>
        /// Supprime une categorie
        /// </summary>
        /// <param name="categoryId">L'id de la categorie</param>
        public static void DeleteCategory(int catId)
        {
            using (Infrastructure.DB.MySecondBrainContext db = new Infrastructure.DB.MySecondBrainContext())
            {
                Infrastructure.DB.Dossier dossier = db.Dossiers.Find(catId);
                List<Infrastructure.DB.Note> notes = db.Notes.Where(n => n.Iddossier == catId).ToList();
                if (dossier != null && notes.Count == 0)
                {
                    db.Dossiers.Remove(dossier);
                    db.SaveChanges();
                } else if (notes.Count != 0)
                {
                    Console.WriteLine("This category cannot be deleted. Some notes are linked to it.");
                }
            }
        }



    }
}
