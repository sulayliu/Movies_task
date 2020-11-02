using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;

namespace Movies_task2
{
    class Program
    {
        static MoviesDBEntities1 db = new MoviesDBEntities1();
        public static void InsertUsers()
        {
            string path = @"C:\Users\suley\Desktop\Data\Users.txt";

            using (StreamReader sr = File.OpenText(path))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    var info = line.Split('|');
                    int id = int.Parse(info[0]);
                    int age = int.Parse(info[1]);
                    string gender = info[2];
                    string occupation = info[3];
                    SqlParameter p1 = new SqlParameter("@userId",id);
                    SqlParameter p2 = new SqlParameter("@age", age);
                    SqlParameter p3 = new SqlParameter("@gender", gender);
                    SqlParameter p4 = new SqlParameter("@occupation", occupation);
                    db.Database.ExecuteSqlCommand("[dbo].[InsertIntoUser] @userId,@age,@gender,@occupation",p1,p2,p3,p4);
                }
                sr.Close();
            }
        }

        public static void InsertMovies()
        {
            string path = @"C:\Users\suley\Desktop\Data\Movies.txt";

            using (StreamReader sr = File.OpenText(path))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    var info = line.Split('|');
                    int id = int.Parse(info[0]);
                    string name = info[1];
                    SqlParameter p1 = new SqlParameter("@movieId", id);
                    SqlParameter p2 = new SqlParameter("@name", name);
                    db.Database.ExecuteSqlCommand("[dbo].[InsertIntoMovies] @movieId,@name", p1, p2);
                }
                sr.Close();
            }
        }

        public static void InsertRatings()
        {
            string path = @"C:\Users\suley\Desktop\Data\Ratings.txt";
            int counter = 0;
            using (StreamReader sr = File.OpenText(path))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    var info = line.Split('\t');
                    SqlParameter p1 = new SqlParameter("@userId", int.Parse(info[0]));
                    SqlParameter p2 = new SqlParameter("@movieId", int.Parse(info[1]));
                    SqlParameter p3 = new SqlParameter("@RatingNum", int.Parse(info[2]));
                    db.Database.ExecuteSqlCommand("[dbo].[InserIntoRating] @userId,@movieId,@RatingNum", p1, p2,p3);
                    counter++;
                    Console.WriteLine(counter);
                }
                sr.Close();
            }
        }

        public static void GetAllMoviesRatedByAUser(int id)
        {
            SqlParameter userid = new SqlParameter("@userId", id);
            var result = db.Database.SqlQuery<string>("[dbo].[AllMoviesRatedByAUser] @userId",userid).ToList();
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
        }

        public static void GetAverageRatingOfAMovie(int id)
        {
            SqlParameter movieid = new SqlParameter("@movieId", id);
            var result = db.Database.SqlQuery<int>("[dbo].[AverageRatingOfAMovie] @movieId", movieid).ToList();
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
        }
        public static void AverageRatingOfAllMovie()
        {
            var result = db.Database.SqlQuery<AverageRatingOfAllMovies_Result>("[dbo].[AverageRatingOfAllMovies]").ToList();
            foreach (var item in result)
            {
                Console.WriteLine(item.Name + " average rating is : " + item.avg_rating);
            }
        }

        public static void GetTopAverageRatedMovie()
        {
            var result = db.Database.SqlQuery<string>("[dbo].[TopAverageRatedMovie]").ToList();
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
        }

        public static void GetTop10AverageRatedMovie()
        {
            var result = db.Database.SqlQuery<string>("[dbo].[Top10AverageRatedMovie]").ToList();
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
        }

        public static void GetWorstMovie()
        {
            var result = db.Database.SqlQuery<string>("[dbo].[WorstMovie]").ToList();
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
        }

        public static void UpdateMoviesReleasedDate()
        {
            string path = @"C:\Users\suley\Desktop\Data\Movies.txt";

            using (StreamReader sr = File.OpenText(path))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    var info = line.Split('|');
                    int id = int.Parse(info[0]);
                    var release = info[2];
                    SqlParameter p1 = new SqlParameter("@movieId", id);
                    SqlParameter p2 = new SqlParameter("@releaseddate", release);
                    db.Database.ExecuteSqlCommand("[dbo].[UpdateMoviesReleasedDate] @movieId,@releaseddate", p1, p2);
                }
                sr.Close();
            }
        }
        static void Main(string[] args)
        {
            //InsertUsers();
            //InsertMovies();
            //InsertRatings();

            // All The Movies Rated by a User
            Console.WriteLine("Please Enter A user Id: ");
            int id = int.Parse(Console.ReadLine());
            GetAllMoviesRatedByAUser(id);

            // Average rating for a movie
            Console.WriteLine("Please Enter A Movie Id: ");
            int movieId = int.Parse(Console.ReadLine());
            GetAverageRatingOfAMovie(movieId);

            // Average rating for all movies (for each movie)
            AverageRatingOfAllMovie();

            // Top average rated movie
            GetTopAverageRatedMovie();

            // Top 10-average-rated Movies
            GetTop10AverageRatedMovie();

            // Worst Movie (The movie with lowest average rating)
            GetWorstMovie();

            //UPDATE all movies to include their date from the file
            UpdateMoviesReleasedDate();

            Console.ReadKey();
        }
    }
}
