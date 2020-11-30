using System;
using Xunit;
using DodgeyMovieCo;

namespace XUnitTestDodgeyMovies
{
    public class Movie_Tests
    {
        Movie m1 = new DodgeyMovieCo.Movie();

        //*************************************************** TEST NUMBER 1 ********************************************************

        //Test 1 Case 1 - Check that the function returns an int?
        [Theory]
        [InlineData(50, 12445)]
        [InlineData(20, 862)]
        [InlineData(17, 552)]
        public void NumActors_CheckIntReturned_Success(int expected, int movieNum)
        {
            //something along the lines of isString
            Assert.Equal(expected, m1.NumActors(movieNum));
        }

        //Test 1 Case 2 - Happy Test - information eneteed is equal to the output
        [Theory]
        [InlineData(50, 12445)]
        [InlineData(20, 862)]
        [InlineData(17, 552)]
        public void NumActors_CorrectInput_CorrectResult(int expected, int movieNum)
        {
            Assert.Equal(expected, m1.NumActors(movieNum));
        }


        //Test 1 Case 3 - Input is not a number - should throw an exception
        [Theory]
        [InlineData(50, 12445)]
        [InlineData(20, 862)]
        [InlineData(17, 552)]
        public void NumActors_InputIsString_ThrowAnExcepion(int expected, int movieNum)
        {
            Assert.Equal(expected, m1.NumActors(movieNum));
        }

        //Test 1 Case 4 - Input is a number - but a negative one
        [Theory]
        [InlineData(50, 12445)]
        [InlineData(20, 862)]
        [InlineData(17, 552)]
        public void NumActors_InputNegNum_ThrowsEx(int expected, int movieNum)
        {
            Assert.Equal(expected, m1.NumActors(movieNum));
        }

        //Test 1 Case 5 - Case 5 - Input is outside range of int
        [Theory]
        [InlineData(50, 12445)]
        [InlineData(20, 862)]
        [InlineData(17, 552)]
        public void NumActors_InputOutsideRange_ThrowsEx(int expected, int movieNum)
        {
            Assert.Equal(expected, m1.NumActors(movieNum));
        }

        //*************************************************** TEST NUMBER 2 ********************************************************


        //Test 2  case 1  - Check that the function returns an int?
        [Theory]
        [InlineData(50, 12445)]
        [InlineData(-29, 12445)]

        public void MovieAge_CheckIntReturned_Success(int expected, int movieNum)
        {
            //something like IsInt()?
            Assert.Equal(expected, m1.GetAge(movieNum));
        }

        //Test 2  case 2  - Happy Test - information entered is in the db correct output
        [Theory]
        [InlineData(50, 12445)]
        [InlineData(-29, 12445)]

        public void MovieAge_CorrectInput_CorreectOutput(int expected, int movieNum)
        {
            Assert.Equal(expected, m1.GetAge(movieNum));
        }

        //Test 2  case 3 - Input is a spcial char or NaN- should throw an exception
        [Theory]
        [InlineData(50, 12445)]
        [InlineData(-29, 12445)]

        public void MovieAge_InputNaN_ThrowsEx(int expected, int movieNum)
        {
            Assert.Equal(expected, m1.GetAge(movieNum));
        }

        //Test 2  Case 4 - Input is a number - but a negative one
        [Theory]
        [InlineData(50, 12445)]
        [InlineData(-29, 12445)]

        public void MovieAge_InputNegNum_ThrowsEx(int expected, int movieNum)
        {
            Assert.Equal(expected, m1.GetAge(movieNum));
        }

        //Test 2  Case 5 - Input is outside range of int
        [Theory]
        [InlineData(50, 12445)]
        [InlineData(-29, 12445)]

        public void MovieAge_InputOutsideRange_ThrowsEx(int expected, int movieNum)
        {
            Assert.Equal(expected, m1.GetAge(movieNum));
        }

    }
}