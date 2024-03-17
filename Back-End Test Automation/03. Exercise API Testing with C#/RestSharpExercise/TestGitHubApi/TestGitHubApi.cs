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
        private static string repo;
        private static int lastCreatedIssueNumber;
        private static int lastCreatedCommentId;

        [SetUp]
        public void Setup()
        {            
            client = new GitHubApiClient("https://api.github.com/repos/testnakov/", "QA-Automation-Testing-Demo", "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");
            repo = "test-nakov-repo";
        }


        [Test, Order (1)]
        public void Test_GetAllIssuesFromARepo()
        {
			// Act
			var issues = client.GetAllIssues(repo);

			// Assert
			Assert.That(issues, Is.Not.Null, "The issues collection should not be null.");
			Assert.That(issues, Has.Count.GreaterThan(0), "There should be at least one issue.");

			foreach (Issue issue in issues)
			{
				Assert.That(issue.Id, Is.GreaterThan(0), "Issue ID should be greater than 0.");
				Assert.That(issue.Number, Is.GreaterThan(0), "Issue Number should be greater than 0.");
				Assert.That(issue.Title, Is.Not.Empty, "Issue title should not be empty.");
			}
		}

        [Test, Order (2)]
        public void Test_GetIssueByValidNumber()
        {
            // Act
            int issueNumber = 1;
            var issue = client.GetIssueByNumber(repo, issueNumber);

            // Assert
            Assert.That(issue, Is.Not.Null,"The response should contain issue data.");
			Assert.That(issue.Id, Is.GreaterThan(0), "Issue ID should be a positive number.");
			Assert.That(issue.Number, Is.EqualTo(issueNumber), "Issue Number should match the requested number.");
			Assert.That(issue.Title, Is.Not.Empty, "Issue title should not be empty.");
		}
        
        [Test, Order (3)]
        public void Test_GetAllLabelsForIssue()
        {
			// Act
			int issueNumber = 5;
			var labels = client.GetAllLabelsForIssue(repo, issueNumber);

            // Assert
            Assert.That(labels.Count, Is.GreaterThan(0));

            foreach (var label in labels)
            {
                Assert.That(label.Id, Is.GreaterThan(0), "Label ID should be greather than 0.");
                Assert.That(label.Name, Is.Not.Empty, "Label name should be not empty.");

                Console.WriteLine("Label: " + label.Id + " - Name: " + label.Name);
            }

        }

		[Test, Order (4)]
        public void Test_GetAllCommentsForIssue()
        {
			int issueNumber = 6;
			var comments = client.GetAllCommentsForIssue(repo, issueNumber);

			Assert.That(comments.Count, Is.GreaterThan(0));

			foreach (var comment in comments)
			{
				Assert.That(comment.Id, Is.GreaterThan(0), "Comment ID should be greather than 0.");
				Assert.That(comment.Body, Is.Not.Empty, "Comment body should be not empty.");

				Console.WriteLine("Comment: " + comment.Id + " - Name: " + comment.Body);
			}
		}

        [Test, Order(5)]
        public void Test_CreateGitHubIssue()
        {
            string title = "New Issue Title";
            string body = "Body of the new Issue";
            var issue = client.CreateIssue(repo, title, body);
            Assert.Multiple(() =>
            {
                Assert.That(issue.Id, Is.GreaterThan(0));
                Assert.That(issue.Number, Is.GreaterThan(0));
                Assert.That(issue.Title, Is.EqualTo(title));
                Assert.That(issue.Title, Is.Not.Empty);

            });
			
            Console.WriteLine(issue.Number);
            lastCreatedIssueNumber = issue.Number;
		}

        [Test, Order (6)]
        public void Test_CreateCommentOnGitHubIssue()
        {
            int issueNumber = 5774;
            string body = "Body of the new Comment";

            var comment = client.CreateCommentOnGitHubIssue(repo,issueNumber, body);

            Assert.That(comment.Body, Is.EqualTo(body));

            Console.WriteLine(comment.Id);
            lastCreatedCommentId = comment.Id;

        }

        [Test, Order (7)]
        public void Test_GetCommentById()
        {
            int commentId = 1986255221;

			var comment = client.GetCommentById(repo, commentId);

            Assert.IsNotNull(comment);
            Assert.That(commentId, Is.EqualTo(commentId));
		}


        [Test, Order (8)]
        public void Test_EditCommentOnGitHubIssue()
        {
			int commentId = 1986255221;
            string newBody = "Edited comment on this issue";
			var comment = client.EditCommentOnGitHubIssue(repo, commentId, newBody);

			Assert.IsNotNull(comment);
			Assert.That(commentId, Is.EqualTo(commentId));
			Assert.That(comment.Body, Is.EqualTo(newBody));
		}

        [Test, Order (9)]
        public void Test_DeleteCommentOnGitHubIssue()
        {
            int commentId = lastCreatedCommentId;

            bool result = client.DeleteCommentOnGitHubIssue(repo, commentId); 
            
            Assert.IsTrue(result);


        }


    }
}

