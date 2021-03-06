﻿using Eduria;
using Eduria.Services;
using EduriaData.Models.ExamLayer;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace EduriaTest
{
    public class AnswerServiceTest
    {
        private DbContextOptions<EduriaContext> Options;

        public AnswerServiceTest()
        {
            Options = new DbContextOptionsBuilder<EduriaContext>().
                UseInMemoryDatabase(databaseName: "Eduria_Development").
                Options;
        }

        /// <summary>
        /// Method for Mocking generic DbSets.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="elements">IEnumberable filled with the object(s).</param>
        /// <returns>A Mock of the specific DbSet.</returns>
        public static Mock<DbSet<T>> CreateDbSetMock<T>(IEnumerable<T> elements) where T : class
        {
            var elementsAsQueryable = elements.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(elementsAsQueryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elementsAsQueryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elementsAsQueryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(elementsAsQueryable.GetEnumerator());

            return dbSetMock;
        }

        [Fact]
        public void GetAllTest()
        {
            //Arrange 
            var answerList = new List<Answer>
            {
                new Answer { AnswerId = 1 },
                new Answer { AnswerId = 2 },
                new Answer { AnswerId = 3 },
                new Answer { AnswerId = 4 }
            }.AsQueryable();

            var answerMockSet = CreateDbSetMock(answerList);

            //Act
            var contextMock = new Mock<EduriaContext>(Options);
            contextMock.Setup(x => x.Answers).Returns(answerMockSet.Object);

            var service = new AnswerService(contextMock.Object);
            var answers = service.GetAll();

            //Assert
            Assert.Equal(answerList.Count(), answers.Count());
        }

        [Fact]
        public void GetByIdTest()
        {
            //Arrange
            Answer answer = new Answer
            {
                AnswerId = 1,
                Correct = 1,
                QuestionId = 1,
                Text = "Dit is een testvraag."
            };

            var contextMock = new Mock<EduriaContext>(Options);
            contextMock.Setup(x => x.Answers.Find(1)).Returns(answer);

            var service = new AnswerService(contextMock.Object);
            Answer answerById = service.GetById(1);

            //Assert
            Assert.Equal(answer.AnswerId, answerById.AnswerId);
            Assert.Equal(answer.Correct, answerById.Correct);
            Assert.Equal(answer.QuestionId, answerById.QuestionId);
            Assert.Equal(answer.Text, answerById.Text);
        }

        [Fact]
        public void GetAnswersByQuestionsListTest()
        {
            //Arrange
            var answerList = new List<Answer>
            {
                new Answer { AnswerId = 1, QuestionId = 1, Correct = 0, Text = "Dit is antwoord 1" },
                new Answer { AnswerId = 2, QuestionId = 2, Correct = 0, Text = "Dit is antwoord 2" },
                new Answer { AnswerId = 3, QuestionId = 3, Correct = 0, Text = "Dit is antwoord 3" },
                new Answer { AnswerId = 4, QuestionId = 4, Correct = 0, Text = "Dit is antwoord 4" }
            }.AsQueryable();

            var questionList = new List<Question>
            {
                new Question { QuestionId = 1, Text = "Dit is vraag 1" },
                new Question { QuestionId = 2, Text = "Dit is vraag 2" },
                new Question { QuestionId = 3, Text = "Dit is vraag 3" },
                new Question { QuestionId = 4, Text = "Dit is vraag 4" }
            }.AsQueryable();

            var answerMockSet = CreateDbSetMock(answerList);

            //Act
            var contextMock = new Mock<EduriaContext>(Options);
            contextMock.Setup(x => x.Answers).Returns(answerMockSet.Object);

            var service = new AnswerService(contextMock.Object);
            var tempAnswerList = service.GetAnswersByQuestionsList(questionList);

            //Assert
            Assert.NotEmpty(tempAnswerList);
            Assert.Equal(4, tempAnswerList.Count());
        }
    }
}
