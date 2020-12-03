﻿using DodgeyMovieCo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DodgeyMovieCo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class MovieController : ControllerBase
    {
        public static List<Movie> staticResultsHolder = new List<Movie>();

        IConfiguration configuration;
        // have to add this using nuget sqldataclient
        SqlConnectionStringBuilder stringBuilder = new SqlConnectionStringBuilder();

        public string connectionString;

        public MovieController(IConfiguration iConfig)
        {
            this.configuration = iConfig;

            this.stringBuilder.DataSource = this.configuration.GetSection("DBConnectionStrings").GetSection("Url").Value;
            this.stringBuilder.InitialCatalog = this.configuration.GetSection("DBConnectionStrings").GetSection("Database").Value;
            this.stringBuilder.UserID = this.configuration.GetSection("DBConnectionStrings").GetSection("User").Value;
            this.stringBuilder.Password = this.configuration.GetSection("DBConnectionStrings").GetSection("Password").Value;
            this.connectionString = this.stringBuilder.ConnectionString;
        }


        // GET: api/<MovieController>/NumActors

        [Route("NumActors/{movieNum}")]
        [HttpGet]
        public List<NumActorsResponseModel> ActorTotal(int movieNum)
        {
            Movie m2 = new Movie();
            return m2.NumActors(connectionString, movieNum);
        }



        // GET: api/<MovieController>/MovieAge
        [Route("MovieAge/{movieTitle}")]
        [HttpGet]
        public int GetMovieAge(string movieTitle)
        {
            Movie m2 = new Movie();
            return m2.GetAge(connectionString, movieTitle);

        }


        //ReadTask1
        // GET: api/<MovieController>/AllMovies
        [Route("AllMovies")]
        [HttpGet]
        public List<Movie> GetAllMovies()
        {
            MovieDataBseResponseModel movie1 = new MovieDataBseResponseModel();
            Movie m1 = new Movie();
            

            string query1 = "select * from Movie";

            // create connection and command
            SqlConnection connecting = new SqlConnection(connectionString);

            SqlCommand getActorsCmd = new SqlCommand(query1, connecting);
           
            try
            {
                connecting.Open();

                using (SqlDataReader reader = getActorsCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // ORM - Object Relation Mapping
                        movie1.Movies.Add(
                            // major problem here was that float in SQL and float in c# are different - so was throwing a casting error - winratio had to be cast as a "single"
                            new Movie()
                            {
                                MovieNum = Convert.ToInt32(reader[0]),
                                Title = reader[1].ToString(),
                                ReleaseYear = Convert.ToInt32(reader[2]),
                                RunTime = (Convert.ToInt32(reader[3]))
                            });
                    
                    }

                    reader.Close();
                }

                connecting.Close();
            }
            catch (SqlException ex)
            {
                throw new ApplicationException($"Some sql error happened + {ex}");
            }

            staticResultsHolder = movie1.Movies.ToList();
            
            return movie1.Movies;



        }
        
        // GET api/<MovieController>/The
        [Route("TitleContainsThe")]
        [HttpGet]
        public List<string> TitlesThatBeginWith()
        {
            //access the database and display the titles for all the movies 
            //with title that begin with the word “The” (case insensitive)
            MovieDataBseResponseModel movieResposne = new MovieDataBseResponseModel();
            //this is wehre the titles will go after they're pulled out o fthe db'
            List<string> titles = new List<string>();

            string query1 = "select * from Movie " +
                            $"where LOWER m.title like LOWER ('%The%')";
                           
            // create connection and command
            SqlConnection connecting = new SqlConnection(connectionString);

            SqlCommand getMovies = new SqlCommand(query1, connecting);

            try
            {
                connecting.Open();

                using (SqlDataReader reader = getMovies.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // ORM - Object Relation Mapping
                        movieResposne.Movies.Add(
                            // major problem here was that float in SQL and float in c# are different - so was throwing a casting error - winratio had to be cast as a "single"
                            new Movie()
                            {
                                MovieNum = Convert.ToInt32(reader[0]),
                                Title = reader[1].ToString(),
                                ReleaseYear = Convert.ToInt32(reader[2]),
                                RunTime = (Convert.ToInt32(reader[3]))
                            });

                    }

                    reader.Close();
                }

                connecting.Close();
            }
            catch (SqlException ex)
            {
                throw new ApplicationException($"Some sql error happened + {ex}");
            }

            //using LINQ to filter all the strings out of the list -which wil be only the titles
            var result = movieResposne.Movies.OfType<string>();


            // Loop through the collection and add all those titles to a list and then return them
            foreach (var title in result)
            {
                titles.Add(title);
            }

            return titles;



        }
    

        // GET api/<MovieController>/Luke Wilson
        [Route("LukeWilson")]
        [HttpGet]
        public List<string> getLukeWilsonsMovieTitles() { 

            //access the database and display the titles for all the movies 
            //that Luke Wilson starred in
            MovieDataBseResponseModel movieResposne = new MovieDataBseResponseModel();
            //this is wehre the titles will go after they're pulled out o fthe db'
            List<string> titles = new List<string>();

            string query1 =    "SELECT M.MOVIENO, M.TITLE, M.RELYEAR, M.RUNTIME " +
                               "FROM MOVIE M " +
                               "INNER JOIN CASTING C " +
                               "ON C.MOVIENO = M.MOVIENO " +
                               "INNER JOIN ACTOR A " +
                               "ON C.ACTORNO = A.ACTORNO " +
                               "WHERE c.actorno = 36422 ";
            

            // create connection and command
            SqlConnection connecting = new SqlConnection(connectionString);

            SqlCommand getMovies = new SqlCommand(query1, connecting);

            try
            {
                connecting.Open();

                using (SqlDataReader reader = getMovies.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // ORM - Object Relation Mapping
                        movieResposne.Movies.Add(
                            // major problem here was that float in SQL and float in c# are different - so was throwing a casting error - winratio had to be cast as a "single"
                            new Movie()
                            {
                                MovieNum = Convert.ToInt32(reader[0]),
                                Title = reader[1].ToString(),
                                ReleaseYear = Convert.ToInt32(reader[2]),
                                RunTime = (Convert.ToInt32(reader[3]))
                            });

                    }

                    reader.Close();
                }

                connecting.Close();
            }
            catch (SqlException ex)
            {
                throw new ApplicationException($"Some sql error happened + {ex}");
            }

            //using LINQ to filter all the strings out of the list -which wil be only the titles
            var result = movieResposne.Movies.OfType<string>();


            // Loop through the collection and add all those titles to a list and then return them
            foreach (var title in result)
            {
                titles.Add(title);
            }

            return titles;



        }



       // GET api/<MovieController>/RunningTimes
        [Route(("RunningTimes"))]
        [HttpGet]
        public string GetTotalMovieRunTime()
        {
            //Using the list Movies created in step one, 
            //display the total running time of all movies
            
            //create a list of int's to hold each movies runtimes
            List<int> runningTimeOutput = new List<int>();

            //for each movie pull out theruntime and stick it inside the other list 
            foreach (var movie in staticResultsHolder)
            {
                runningTimeOutput.Add(movie.RunTime);
            }
            //calc all the int's inside the list
            int sumOfAll = runningTimeOutput.Sum();

            return $"the total runtime all of the moveisa in the list is {sumOfAll} mins";
        }

        //update task 1
        // PUT api/<MovieController>/ChangeRuntime
        [Route("ChangeRuntime")]
        [HttpPut]
        public ActionResult<Movie> Put([FromBody] UpdateRuntimeRequestModel userUpdateRequest)
        {
            //1.In your program, provide a way to change a movie’s runtime found by title.
            //New title to be obtained via user input.  Change must be reflected in the DB.
            MovieDataBseResponseModel movie1 = new MovieDataBseResponseModel();

            string query1 = "UPDATE MOVIE " +
                            $"SET RUNTIME = {userUpdateRequest.RunTime} " +
                            $"where LOWER m.title like LOWER ('%{userUpdateRequest.Title}%') " +
                            "Select * from MOVIE " +
                            $"where LOWER m.title like LOWER ('%{userUpdateRequest.Title}%') ";


            // create connection and command
            SqlConnection connecting = new SqlConnection(connectionString);

            SqlCommand changeRuntime = new SqlCommand(query1, connecting);

            try
            {
                connecting.Open();

                using (SqlDataReader reader = changeRuntime.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // ORM - Object Relation Mapping
                        movie1.Movies.Add(
                            // pushing the mapped object intot a new object and pushing to the list.
                            new Movie()
                            {
                                MovieNum = Convert.ToInt32(reader[0]),
                                Title = reader[1].ToString(),
                                ReleaseYear = Convert.ToInt32(reader[2]),
                                RunTime = (Convert.ToInt32(reader[3]))
                            });

                    }

                    reader.Close();
                }

                connecting.Close();
            }
            catch (SqlException ex)
            {
                throw new ApplicationException($"Some sql error happened + {ex}");
            }

            return Ok(movie1.Movies);
        }
        
        
        // PUT api/<MovieController>/DeppJohnny
        [Route("ChangeActorName")]
        [HttpPut]
        public ActionResult<Actor> Put([FromBody] UpdateActorNameRequest updateActorName)
        {
            //Provide a way to change an actor’s surname and fullname, found by givenname and surname. 
            //New surname to obtained via user input.Change must be reflected in the DB.
            ActorResponseModel actorResponse = new ActorResponseModel();


            string setName = "UPDATE ACTOR A " +
                             $"SET A.SURNAME = {updateActorName.NewSurname} " +
                             $"SET A.FULLNAME = A.GIVENNAME + ' ' + '{updateActorName.NewSurname}' " +
                             $"WHERE A.GIVENNAME = {updateActorName.GivenName} AND A.SURNAME = {updateActorName.Surname}" +
                             " GO " + 
                             "SELECT * FROM ACTOR " +
                             $"WHERE A.GIVENNAME = { updateActorName.GivenName} AND A.SURNAME = { updateActorName.Surname} ";
            


            // create connection and command
            SqlConnection connecting = new SqlConnection(connectionString);
            SqlCommand setActorNameUsingSurnameAndFullname = new SqlCommand(setName, connecting);

            try
            {
                connecting.Open();

                using (SqlDataReader reader = setActorNameUsingSurnameAndFullname.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // ORM - Object Relation Mapping
                        actorResponse.Actors.Add(
                                new Actor()
                                {
                                    ActorNo = Convert.ToInt32(reader[0]),
                                    FullName = reader[1].ToString(),
                                    GivenName = reader[2].ToString(),
                                    Surname = reader[3].ToString()
                                });

                    }
                    reader.Close();
                }

                connecting.Close();
            }
            catch (SqlException ex)
            {
                throw new ApplicationException($"Some sql error happened + {ex}");
            }



            return Ok(actorResponse.Actors);


        }





        /*
        // POST api/<MovieController>/CreateNewMovie
        [HttpPost]
        public ActionResult<Movie> Post([FromBody] Movie newMovie)
        {
            return Ok(newMovie);
        }

        // POST api/<MovieController>/NewActor
        [HttpPost]
        public ActionResult<Actor> Post([FromBody] Actor newActor)
        {
            return Ok(newActor);
        }

        // POST api/<MovieController>/CastActorIntoNewMovie
        [HttpPost]
        public ActionResult<Movie> Post([FromBody] Actor actor, Movie targetMovie)
        {
            return Ok(targetMovie);
        }
        
        */
    }
}
