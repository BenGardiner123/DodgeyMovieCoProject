﻿using DodgeyMovieCo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DodgeyMovieCo.MovieClassLb
{
    public class DatabaseLayer
    {

        
        IConfiguration configuration;
        // have to add this using nuget sqldataclient
        SqlConnectionStringBuilder stringBuilder = new SqlConnectionStringBuilder();
        public string connectionString;



        public DatabaseLayer(IConfiguration iConfig)
        {
            
            this.configuration = iConfig;
            this.stringBuilder.DataSource = this.configuration.GetSection("DBConnectionStrings").GetSection("Url").Value;
            this.stringBuilder.InitialCatalog = this.configuration.GetSection("DBConnectionStrings").GetSection("Database").Value;
            this.stringBuilder.UserID = this.configuration.GetSection("DBConnectionStrings").GetSection("User").Value;
            this.stringBuilder.Password = this.configuration.GetSection("DBConnectionStrings").GetSection("Password").Value;
            this.connectionString = this.stringBuilder.ConnectionString;
        }



        public string dbRedirect()
        {

            SqlConnectionStringBuilder dodgyestringBuilder = new SqlConnectionStringBuilder();
            dodgyestringBuilder.DataSource = "no.database.here.com";
            dodgyestringBuilder.InitialCatalog = "Is";
            dodgyestringBuilder.UserID = "Wally";
            dodgyestringBuilder.Password = "Where";


            // create connection and command
            /*
             implementing this inside the brackets makes sure that all open connections are terminated
             */
            try
            {
                var query = "select * from sys.tables";

                using (SqlConnection conn = new SqlConnection(dodgyestringBuilder.ConnectionString))
                {
                    var command = new SqlCommand(query, conn);
                    conn.Open();
                }
                return "conenction failed";

            }
            //hopefully we do the same thing below but wuth te right details  - then when this fails the code below will execute
            //taken from https://stackoverflow.com/questions/434864/how-to-check-if-connection-string-is-valid
            catch (SqlException ex)
            {

                try
                {
                    
                    using (SqlConnection conn = new SqlConnection(this.connectionString))
                    { 
                        conn.Open();
                        conn.Close();
                        return " There was an inital error with your account however connection redirected succesfully ! - - " + ex.Message;
                    }
                    
                }
                catch (SqlException SqlEx)
                {
                    return "Uh oh those dodgey connection details created a much larger issue " + SqlEx;
                }

                
            }
           


        }

        
        public List<Movie> GetAllMovies()
        {
            List<Movie> Movies = new List<Movie>();
            
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
                        Movies.Add(
                            
                            new Movie()
                            {
                                MovieNum = Convert.ToInt32(reader[0]),
                                Title = reader[1].ToString(),
                                ReleaseYear = Convert.ToInt16(reader[2]),
                                RunTime = Convert.ToInt16(reader[3])
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

            return Movies;
        }


        public List<string> TitlesThatBeginWith()
        {
            //access the database and display the titles for all the movies 
            //with title that begin with the word “The” (case insensitive)
            MovieDatabaseserverRespnse movieResposne = new MovieDatabaseserverRespnse();
            //this is wehre the titles will go after they're pulled out o fthe db'
            List<string> titles = new List<string>();

            string query1 = "select * from Movie M " +
                            "where LOWER (M.TITLE) LIKE LOWER ('%The%')";

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
                                ReleaseYear = Convert.ToInt16(reader[2]),
                                RunTime = Convert.ToInt16(reader[3])
                            });

                    }

                    reader.Close();
                }

                connecting.Close();
            }
            catch (SqlException ex)
            {
                //ToString fullfill the contract you have to return the same type - this will stop the entire show crashing like a bitch
                List<string> message = new List<string>();
                message.Add(ex.Message);
                message.Add("You done fucked up mate!");
                return message;
                
            }

            //using LINQ to filter all the strings out of the list -which wil be only the titles
            var result = movieResposne.Movies;


            // Loop through the collection and add all those titles to a list and then return them
            foreach (var movie in result)
            {
                titles.Add(movie.Title);
            }

            return titles;



        }

        public List<string> getLukeWilsonsMovieTitles()
        {

            //access the database and display the titles for all the movies 
            //that Luke Wilson starred in
            MovieDatabaseserverRespnse movieResposne = new MovieDatabaseserverRespnse();
            //this is wehre the titles will go after they're pulled out o fthe db'
            List<string> titles = new List<string>();

            string query1 = "SELECT M.MOVIENO, M.TITLE, M.RELYEAR, M.RUNTIME " +
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
                                ReleaseYear = Convert.ToInt16(reader[2]),
                                RunTime = Convert.ToInt16(reader[3])
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
            var result = movieResposne.Movies;


            // Loop through the collection and add all those titles to a list and then return them
            foreach (var movie in result)
            {
                titles.Add(movie.Title);
            }

            return titles;



        }

        public string GetTotalMovieRunTime()
        {
            //Using the list Movies created in step one, 
            //display the total running time of all movies

            var output = GetAllMovies();
            //create a list of int's to hold each movies runtimes
            List<int> runningTimeOutput = new List<int>();

            //for each movie pull out theruntime and stick it inside the other list 
            foreach (var movie in output)
            {
                runningTimeOutput.Add(movie.RunTime);
            }
            //calc all the int's inside the list
            int sumOfAll = runningTimeOutput.Sum();

            return $"the total runtime all of the movies in the database is {sumOfAll} mins";
        }

        public string getAMovie(string titleToSearch)
        {
            //Using the list Movies created in step one, 
            //display the total running time of all movies

            var output = GetAllMovies();
            //create a list of int's to hold each movies runtimes
            List<string> MovieList = new List<string>();

            //for each movie pull out theruntime and stick it inside the other list 
            foreach (var movie in output)
            {
                if (movie.Title == titleToSearch)
                {
                    MovieList.Add(movie.Title);
                }
            }
                
            return MovieList[0];
        }

       

        public Movie ChangeMovieRuntime(UpdateRuntimeRequestModel userUpdateRequest)
        {
            //1.In your program, provide a way to change a movie’s runtime found by title.
            //New title to be obtained via user input.  Change must be reflected in the DB.
            MovieDatabaseserverRespnse movieResposne = new MovieDatabaseserverRespnse();

            string query1 = "UPDATE MOVIE " +
                            "SET RUNTIME = @userRuntime " +
                            "where LOWER m.title like LOWER @title " + 
                            "LIMIT 1";
                           


            // create connection and command
            SqlConnection connecting = new SqlConnection(connectionString);


            using (SqlCommand changeRuntime = new SqlCommand(query1, connecting))
            {
                changeRuntime.Parameters.Add("@userRuntime", SqlDbType.VarChar, 50).Value = userUpdateRequest.RunTime;
                changeRuntime.Parameters.Add("@title", SqlDbType.VarChar, 50).Value = userUpdateRequest.Title;

                try
                {
                    connecting.Open();

                    using (SqlDataReader reader = changeRuntime.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // ORM - Object Relation Mapping
                            movieResposne.Movies.Add(
                                // pushing the mapped object intot a new object and pushing to the list.
                                new Movie()
                                {
                                    MovieNum = Convert.ToInt32(reader[0]),
                                    Title = reader[1].ToString(),
                                    ReleaseYear = Convert.ToInt16(reader[2]),
                                    RunTime = Convert.ToInt16(reader[3])
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

                return movieResposne.Movies[0];
            }
        }


        public Actor GetActor(UpdateActorNameRequest updateActorName)
        {
            ActorResponseModel actorResponse = new ActorResponseModel();

            string getActor = "select * From ACTOR  " +
                              "WHERE GIVENNAME = @givename and SURNAME = @surname";

            // create connection and command
            SqlConnection connecting = new SqlConnection(connectionString);
            using (SqlCommand getActorCmd = new SqlCommand(getActor, connecting))
            {
                getActorCmd.Parameters.Add("@newSurname", SqlDbType.VarChar, 100).Value = updateActorName.NewSurname;
                getActorCmd.Parameters.Add("@givename", SqlDbType.VarChar, 100).Value = updateActorName.GivenName;
                getActorCmd.Parameters.Add("@surname", SqlDbType.VarChar, 100).Value = updateActorName.Surname;
                try
                {
                   
                    connecting.Open();

                    using (SqlDataReader reader = getActorCmd.ExecuteReader())
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

            }



            return actorResponse.Actors[0];

        }



       public string UpdateActorName(Actor selectedActor)
        {
            //Provide a way to change an actor’s surname and fullname, found by givenname and surname. 
            //New surname to obtained via user input.Change must be reflected in the DB.
            
            string setName = "UPDATE ACTOR  " +
                             "SET FULLNAME = @fullname, " +
                             "GIVENNAME = @givename, " +
                             "SURNAME = @surname " + 
                             "WHERE ACTORNO = @actorno";

            // create connection and command
            SqlConnection connecting = new SqlConnection(connectionString);
            using (SqlCommand setActorNameUsingSurnameAndFullname = new SqlCommand(setName, connecting))
            {
                setActorNameUsingSurnameAndFullname.Parameters.Add("@fullname", SqlDbType.VarChar, 100).Value = selectedActor.FullName;
                setActorNameUsingSurnameAndFullname.Parameters.Add("@actorno", SqlDbType.Int, 100).Value = selectedActor.ActorNo;
                setActorNameUsingSurnameAndFullname.Parameters.Add("@givename", SqlDbType.VarChar, 100).Value = selectedActor.GivenName;
                setActorNameUsingSurnameAndFullname.Parameters.Add("@surname", SqlDbType.VarChar, 100).Value = selectedActor.Surname;
                try
                {
                    connecting.Open();
                    setActorNameUsingSurnameAndFullname.ExecuteNonQuery();
                    connecting.Close();
                }
                catch (SqlException ex)
                {
                    throw new ApplicationException($"Some sql error happened + {ex.Message} + ' ' +  {ex.ErrorCode} + ' ' + {ex.Number}");
                }

            }


            return "actor updated";


        }


        public int getNextMovieNum()
        {
            //need to contact the db and get the next seqeunece of the movieno
            string query1 = "SELECT MAX(MOVIENO) + 1 FROM MOVIE ";
                    

            SqlConnection connecting = new SqlConnection(connectionString);
            SqlCommand getNextID = new SqlCommand(query1, connecting);

            int output = 0;
            try
            {
                connecting.Open();

                using (SqlDataReader reader = getNextID.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        output = reader.GetInt32(0);
                    }

                    reader.Close();
                }

                connecting.Close();
            }
            catch (SqlException ex)
            {
                throw new ApplicationException($"Some sql error happened + {ex.Message} + {ex.Errors} + {ex.Number}");
            }
            return output;
        }

        public int getNextActorNum()
        {
            //need to contact the db and get the next seqeunece of the movieno
            string query1 = "SELECT MAX(ACTORNO) + 1 FROM ACTOR ";

            SqlConnection connecting = new SqlConnection(connectionString);
            SqlCommand getNextID = new SqlCommand(query1, connecting);

            int output = 0;
            try
            {
                connecting.Open();

                using (SqlDataReader reader = getNextID.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        output = reader.GetInt32(0);
                    }

                    reader.Close();
                }

                connecting.Close();
            }
            catch (SqlException ex)
            {
                throw new ApplicationException($"Some sql error happened + {ex.Message} + {ex.Errors} + {ex.Number}");
            }
            return output;
        }

        public int getNextcastingNum()
        {
            //need to contact the db and get the next seqeunece of the movieno
            string query1 = "SELECT MAX(castid) + 1 FROM casting ";

            SqlConnection connecting = new SqlConnection(connectionString);
            SqlCommand getNextID = new SqlCommand(query1, connecting);

            int output = 0;
            try
            {
                connecting.Open();

                using (SqlDataReader reader = getNextID.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        output = reader.GetInt32(0);
                    }

                    reader.Close();
                }

                connecting.Close();
            }
            catch (SqlException ex)
            {
                throw new ApplicationException($"Some sql error happened + {ex.Message} + {ex.Errors} + {ex.Number}");
            }
            return output;
        }



        public NewMovieRequestModel CreateNewMovie(int nextValue, NewMovieRequestModel newUserMovie)
        {
       
            string query1 = "INSERT INTO MOVIE (MOVIENO, TITLE, RELYEAR, RUNTIME) " +
                           "VALUES (@movienum, @newtitle, @releaseYear, @runTime) ";


            // create connection and command
            SqlConnection connecting = new SqlConnection(connectionString);

            SqlCommand createNewMovie = new SqlCommand(query1, connecting);
            createNewMovie.Parameters.Add("@movienum", SqlDbType.Int, 100).Value = nextValue;
            createNewMovie.Parameters.Add("@newtitle", SqlDbType.VarChar, 100).Value = newUserMovie.Title;
            createNewMovie.Parameters.Add("@releaseYear", SqlDbType.SmallInt).Value = newUserMovie.ReleaseYear;
            createNewMovie.Parameters.Add("@runTime", SqlDbType.SmallInt).Value = newUserMovie.RunTime;

            try
            {
                connecting.Open();
                createNewMovie.ExecuteNonQuery();
                connecting.Close();
            }
            catch (SqlException ex)
            {
                throw new ApplicationException($"Some sql error happened + {ex} + { ex.Message} + { ex.Errors} + { ex.Number}");
            }

            var outputMovie = newUserMovie;
            outputMovie.MovieNum = nextValue;

            return outputMovie;
        }


        public Actor CreateNewactor(Actor newActor)
        {


            string query1 = "INSERT INTO Actor (ActorNo, FullName, GivenName, Surname) " +
                           "VALUES (@actorno, @fullname, @givenname, @surname) ";

            // create connection and command
            SqlConnection connecting = new SqlConnection(connectionString);

            SqlCommand createNewActor = new SqlCommand(query1, connecting);
            createNewActor.Parameters.Add("@actorno", SqlDbType.Int, 100).Value = newActor.ActorNo;
            createNewActor.Parameters.Add("@fullname", SqlDbType.VarChar, 100).Value = newActor.FullName;
            createNewActor.Parameters.Add("@givenname", SqlDbType.VarChar, 100).Value = newActor.GivenName;
            createNewActor.Parameters.Add("@surname", SqlDbType.VarChar, 100).Value = newActor.Surname;

            try
            {
                connecting.Open();
                createNewActor.ExecuteNonQuery();
                connecting.Close();
            }
            catch (SqlException ex)
            {
                throw new ApplicationException($"Some sql error happened + {ex} + { ex.Message} + { ex.Errors} + { ex.Number}");
            }

            return newActor;
        }


        public List<string> checkCasting(int ActorNo)
        {

            //access the database and display the titles for all the movies 
            //that Luke Wilson starred in
            MovieDatabaseserverRespnse movieResposne = new MovieDatabaseserverRespnse();
            //this is wehre the titles will go after they're pulled out o fthe db'
            List<string> titles = new List<string>();

            string query1 = "SELECT M.MOVIENO, M.TITLE, M.RELYEAR, M.RUNTIME " +
                               "FROM MOVIE M " +
                               "INNER JOIN CASTING C " +
                               "ON C.MOVIENO = M.MOVIENO " +
                               "INNER JOIN ACTOR A " +
                               "ON C.ACTORNO = A.ACTORNO " +
                               "WHERE c.actorno = @actorno ";


            // create connection and command
            SqlConnection connecting = new SqlConnection(connectionString);

            SqlCommand checkCasting = new SqlCommand(query1, connecting);
            checkCasting.Parameters.Add("@actorno", SqlDbType.Int, 100).Value = ActorNo;

            try
            {
                connecting.Open();

                using (SqlDataReader reader = checkCasting.ExecuteReader())
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
                                ReleaseYear = Convert.ToInt16(reader[2]),
                                RunTime = Convert.ToInt16(reader[3])
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
            var result = movieResposne.Movies;


            // Loop through the collection and add all those titles to a list and then return them
            foreach (var movie in result)
            {
                titles.Add(movie.Title);
            }

            return titles;



        }

        public Casting CastActorIntoMovie([FromBody] Casting newCasting)
        {


            string query1 = "INSERT INTO CASTING (CASTID, ACTORNO, MOVIENO) " +
                           "VALUES (@castingid, @actorno, @movieno) ";

            // create connection and command
            SqlConnection connecting = new SqlConnection(connectionString);

            SqlCommand createNewCasting = new SqlCommand(query1, connecting);
            createNewCasting.Parameters.Add("@castingid", SqlDbType.Int, 100).Value = newCasting.CastID;
            createNewCasting.Parameters.Add("@actorno", SqlDbType.Int, 100).Value = newCasting.ActorNo;
            createNewCasting.Parameters.Add("@movieno", SqlDbType.Int, 100).Value = newCasting.MovieNo;
            try
            {
                connecting.Open();
                createNewCasting.ExecuteNonQuery();
                connecting.Close();
            }
            catch (SqlException ex)
            {
                throw new ApplicationException($"Some sql error happened + {ex} + { ex.Message} + { ex.Errors} + { ex.Number}");
            }

            return newCasting;
        }


    }
}
