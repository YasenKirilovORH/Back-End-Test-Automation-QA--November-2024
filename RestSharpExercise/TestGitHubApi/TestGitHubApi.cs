using RestSharpServices;
using System.Net;
using System.Reflection.Emit;
using System.Text.Json;
using RestSharp;
using RestSharp.Authenticators;
using NUnit.Framework.Internal;
using RestSharpServices.Models;
using System;

namespace TestGitHubApi
{
    public class TestGitHubApi
    {
        private GitHubApiClient client;
        string repoName = "test-nakov-repo";
        int lastCreatedIssueNumber;
        long lastCreatedCommentId;
        

        [SetUp]
        public void Setup()
        {           
            // We should put our Git username and Token
            client = new GitHubApiClient("https://api.github.com/repos/testnakov/", "username", "token");
        }


        [Test, Order (1)]
        public void Test_GetAllIssuesFromARepo()
        {
            // Act
            var issues = client.GetAllIssues(repoName);

            // Assert
            Assert.That(issues, Has.Count.GreaterThan(1), "There should be more than one issue.");

            foreach (var issue in issues)
            {
                Assert.That(issue.Id, Is.GreaterThan(0), "Issue ID should be greater than 0.");
                Assert.That(issue.Number, Is.GreaterThan(0), "Issue Number should be greater than 0.");
                Assert.That(issue.Title, Is.Not.Empty, "Issue Title should not be empty.");
            }
        }

        [Test, Order (2)]
        public void Test_GetIssueByValidNumber()
        {
            // Arrange
            int issueNumber = 1;

            // Act
            var issue = client.GetIssueByNumber(repoName, issueNumber);

            // Assert
            Assert.IsNotNull(issue, "The response should contain issue data.");
            Assert.That(issue.Id, Is.GreaterThan(0), "Issue ID should be greater than 0.");
            Assert.That(issue.Number, Is.EqualTo(issueNumber), "The issue Number should match the request number.");

        }
        
        [Test, Order (3)]
        public void Test_GetAllLabelsForIssue()
        {
            // Arrange
            int issueNumber = 6;

            // Act
            var labels = client.GetAllLabelsForIssue(repoName, issueNumber);

            // Assert
            Assert.That(labels.Count, Is.GreaterThan(0), "Labels count should be greater than 0.");

            foreach (var label in labels)
            {
                Assert.That(label.Id, Is.GreaterThan(0), "Label ID should be greater than 0.");
                Assert.That(label.Name, Is.Not.Empty, "Label name should not be empty.");

                Console.WriteLine("Label: " + label.Id + " - Name: " + label.Name);
            }
        }

        [Test, Order (4)]
        public void Test_GetAllCommentsForIssue()
        {
            // Arrange
            int issueNumber = 6;

            // Act
            var comments = client.GetAllCommentsForIssue(repoName, issueNumber);

            // Assert
            Assert.That(comments.Count, Is.GreaterThan(0), "Commnets count should be greater than 0");
            
            foreach (var comment in comments)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(comment.Id, Is.GreaterThan(0), "Comment ID should be greater than 0");
                    Assert.That(comment.Body, Is.Not.Empty, "Comment Body should not be empty");

                    Console.WriteLine("Comment: " + comment.Id + " - Body: " + comment.Body);
                });
            }
        }

        [Test, Order(5)]
        public void Test_CreateGitHubIssue()
        {
            // Arrange
            string expectedTitle = "Some random title for testing purpouse";
            string expectedBody = "Some random body";

            // Act
            var issue = client.CreateIssue(repoName, expectedTitle, expectedBody);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(issue.Id, Is.GreaterThan(0), "Issue ID should be greater than 0.");
                Assert.That(issue.Number, Is.GreaterThan(0), "Issue Number should be greater than 0");
                Assert.That(issue.Title, Is.EqualTo(expectedTitle), "Issue Title should match the expected title.");
                Assert.That(issue.Body, Is.EqualTo(expectedBody), "Issue Body should match the expected body.");
            });

            Console.WriteLine(issue.Number);
            lastCreatedIssueNumber = issue.Number;

        }

        [Test, Order (6)]
        public void Test_CreateCommentOnGitHubIssue()
        {
            // Arrange
            string expectedCommentBody = "New comment.";

            // Act
            var comment = client.CreateCommentOnGitHubIssue(repoName, lastCreatedIssueNumber, expectedCommentBody);

            // Assert
            Assert.That(comment.Body, Is.EqualTo(expectedCommentBody), "Comment Body should match the expected body.");
            Console.WriteLine(comment.Id);
            lastCreatedCommentId = comment.Id;
        }

        [Test, Order (7)]
        public void Test_GetCommentById()
        {
            // Act
            var comment = client.GetCommentById(repoName, lastCreatedCommentId);

            // Assert
            Assert.IsNotNull(comment, "Comment should not be empty.");
            Assert.That(comment.Id, Is.EqualTo(lastCreatedCommentId), "The retrivet comment ID should match the requested comment ID");
        }


        [Test, Order (8)]
        public void Test_EditCommentOnGitHubIssue()
        {
            // Arrange
            string newCommentBody = "Updated body for the comment.";

            // Act
            var updatedComment = client.EditCommentOnGitHubIssue(repoName, lastCreatedCommentId, newCommentBody);

            // Assert
            Assert.IsNotNull(updatedComment, "The updated comment should not be null.");
            Assert.That(updatedComment.Id, Is.EqualTo(lastCreatedCommentId), "The updated comment ID should match the original comment ID.");
            Assert.That(updatedComment.Body, Is.EqualTo(newCommentBody), "The updated body text should match the new body text.");
        }

        [Test, Order (9)]
        public void Test_DeleteCommentOnGitHubIssue()
        {
            // Act
            var result = client.DeleteCommentOnGitHubIssue(repoName, lastCreatedCommentId);

            // Assert
            Assert.IsTrue(result, "The comment should be sucessfully deleted.");
        }


    }
}

