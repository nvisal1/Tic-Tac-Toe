using ClientRestSDKLibrary;
using ClientRestSDKLibrary.Models;
using Microsoft.Rest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace RestFunctionalTests
{
    [TestClass]
    public class FunctionalTests
    {
        const string LocalEndpointUrl = "https://localhost:44365";
        static ServiceClientCredentials serviceClientCredentials = new TokenCredentials("FakeTokenValue");
        static readonly ClientRestSDKLibraryClient client = new ClientRestSDKLibraryClient(new Uri(LocalEndpointUrl), serviceClientCredentials);

        [TestMethod]
        public void VerifyPlayerXWinner()
        {
            MessagePayload body = new MessagePayload()
            {
                Move = 0,
                AzurePlayerSymbol = "O",
                HumanPlayerSymbol = "X",
                GameBoard = new string[9] {
                    "X", "O", "?",
                    "X", "O", "?",
                    "X", "?", "?"
                }
            };

            ExecuteMoveResponse response = client.ExecuteMove(body);

            Assert.AreEqual("X", response.Winner);
            Assert.IsTrue(new List<int?>() { 0, 3, 6 }.SequenceEqual(response.WinPositions));
            Assert.AreEqual(null, response.Move);
        }

        [TestMethod]
        public void VerifyPlayerOWinner()
        {
            MessagePayload body = new MessagePayload()
            {
                Move = 0,
                AzurePlayerSymbol = "X",
                HumanPlayerSymbol = "O",
                GameBoard = new string[9] {
                    "O", "X", "?",
                    "O", "X", "?",
                    "O", "?", "?"
                }
            };

            ExecuteMoveResponse response = client.ExecuteMove(body);

            Assert.AreEqual("O", response.Winner);
            Assert.IsTrue(new List<int?>() { 0, 3, 6 }.SequenceEqual(response.WinPositions));
            Assert.AreEqual(null, response.Move);
        }

        [TestMethod]
        public void VerifyTie()
        {
            MessagePayload body = new MessagePayload()
            {
                Move = 0,
                AzurePlayerSymbol = "O",
                HumanPlayerSymbol = "X",
                GameBoard = new string[9] {
                    "O", "X", "O",
                    "O", "X", "X",
                    "X", "O", "X"
                }
            };

            ExecuteMoveResponse response = client.ExecuteMove(body);

            Assert.AreEqual("tie", response.Winner);
            Assert.IsNull(response.WinPositions);
            Assert.AreEqual(null, response.Move);
        }

        [TestMethod]
        public void VerifyInconclusive()
        {
            MessagePayload body = new MessagePayload()
            {
                Move = 0,
                AzurePlayerSymbol = "X",
                HumanPlayerSymbol = "O",
                GameBoard = new string[9] {
                    "O", "?", "O",
                    "O", "X", "?",
                    "X", "?", "?"
                }
            };

            ExecuteMoveResponse response = client.ExecuteMove(body);

            Assert.AreEqual("inconclusive", response.Winner);
            Assert.IsNull(response.WinPositions);
            Assert.IsNotNull(response.Move);
        }

        [TestMethod]
        public void VerifyAzureXSymbol()
        {
            MessagePayload body = new MessagePayload()
            {
                Move = 0,
                AzurePlayerSymbol = "X",
                HumanPlayerSymbol = "O",
                GameBoard = new string[9] {
                    "O", "?", "O",
                    "O", "X", "?",
                    "X", "?", "?"
                }
            };

            ExecuteMoveResponse response = client.ExecuteMove(body);

            // AzurePlayerSymbol is set to X,
            // verify that the API added another
            // X to the game board.
            var xIndicies = response.GameBoard
                    .Select((value, index) => new { value, index })
                    .Where((tile) => tile.value.Equals("X"))
                    .Select((tile) => tile.index)
                    .ToArray();

            Assert.AreEqual(3, xIndicies.Length);
        }

        [TestMethod]
        public void VerifyAzureOSymbol()
        {
            MessagePayload body = new MessagePayload()
            {
                Move = 0,
                AzurePlayerSymbol = "O",
                HumanPlayerSymbol = "X",
                GameBoard = new string[9] {
                    "X", "?", "X",
                    "X", "O", "?",
                    "O", "?", "?"
                }
            };

            ExecuteMoveResponse response = client.ExecuteMove(body);

            // AzurePlayerSymbol is set to O,
            // verify that the API added another
            // O to the game board.
            var xIndicies = response.GameBoard
                    .Select((value, index) => new { value, index })
                    .Where((tile) => tile.value.Equals("O"))
                    .Select((tile) => tile.index)
                    .ToArray();

            Assert.AreEqual(3, xIndicies.Length);
        }

        [TestMethod]
        public void VerifySamePlayerSymbols()
        {
            MessagePayload body = new MessagePayload()
            {
                Move = 0,
                AzurePlayerSymbol = "X",
                HumanPlayerSymbol = "X",
                GameBoard = new string[9] {
                    "O", "?", "X",
                    "X", "O", "?",
                    "O", "?", "?"
                }
            };

            try
            {
                ExecuteMoveResponse response = client.ExecuteMove(body);

                // Fail if an HttpOperationException is not thrown
                Assert.IsTrue(false);
            }
            catch (HttpOperationException error)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, error.Response.StatusCode);
                Assert.IsTrue(error.Response.Content.Equals("\"azurePlayerSymbol and humanPlayerSymbol must be opposites\""));
            }
        }

        [TestMethod]
        public void VerifyInvalidHumanPlayerSymbol()
        {
            MessagePayload body = new MessagePayload()
            {
                Move = 0,
                AzurePlayerSymbol = "X",
                HumanPlayerSymbol = "Q",
                GameBoard = new string[9] {
                    "O", "?", "X",
                    "X", "O", "?",
                    "O", "?", "?"
                }
            };

            try
            {
                ExecuteMoveResponse response = client.ExecuteMove(body);

                // Fail if an HttpOperationException is not thrown
                Assert.IsTrue(false);
            }
            catch (HttpOperationException error)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, error.Response.StatusCode);
                Assert.IsTrue(error.Response.Content.Equals("\"humanPlayerSymbol must be either 'O' or 'X'\""));
            }
        }

        [TestMethod]
        public void VerifyInvalidAzurePlayerSymbol()
        {
            MessagePayload body = new MessagePayload()
            {
                Move = 0,
                AzurePlayerSymbol = "Q",
                HumanPlayerSymbol = "",
                GameBoard = new string[9] {
                    "O", "?", "X",
                    "X", "O", "?",
                    "O", "?", "?"
                }
            };

            try
            {
                ExecuteMoveResponse response = client.ExecuteMove(body);

                // Fail if an HttpOperationException is not thrown
                Assert.IsTrue(false);
            }
            catch (HttpOperationException error)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, error.Response.StatusCode);
                Assert.IsTrue(error.Response.Content.Equals("\"azurePlayerSymbol must be either 'O' or 'X'\""));
            }
        }

        [TestMethod]
        public void VerifyInvalidGameBoardSymbols()
        {
            MessagePayload body = new MessagePayload()
            {
                Move = 0,
                AzurePlayerSymbol = "O",
                HumanPlayerSymbol = "X",
                GameBoard = new string[9] {
                    "U", "?", "X",
                    "X", "O", "?",
                    "O", "?", "?"
                }
            };

            try
            {
                ExecuteMoveResponse response = client.ExecuteMove(body);

                // Fail if an HttpOperationException is not thrown
                Assert.IsTrue(false);
            }
            catch (HttpOperationException error)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, error.Response.StatusCode);
                Assert.IsTrue(error.Response.Content.Equals("\"gameBoard can only contain 'X', 'O', or '?'\""));
            }
        }

        [TestMethod]
        public void VerifySmallerGameBoardLength()
        {
            MessagePayload body = new MessagePayload()
            {
                Move = 0,
                AzurePlayerSymbol = "O",
                HumanPlayerSymbol = "X",
                GameBoard = new string[6] {
                    "X", "?", "X",
                    "X", "O", "?"
                }
            };

            try
            {
                ExecuteMoveResponse response = client.ExecuteMove(body);

                // Fail if an HttpOperationException is not thrown
                Assert.IsTrue(false);
            }
            catch (HttpOperationException error)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, error.Response.StatusCode);
                Assert.IsTrue(error.Response.Content.Equals("\"gameboard must have a length of 9 (0 - 8)\""));
            }
        }

        [TestMethod]
        public void VerifyLargerGameBoardLength()
        {
            MessagePayload body = new MessagePayload()
            {
                Move = 0,
                AzurePlayerSymbol = "O",
                HumanPlayerSymbol = "X",
                GameBoard = new string[12] {
                    "X", "?", "X",
                    "X", "O", "?",
                    "X", "O", "?",
                    "X", "O", "?"
                }
            };

            try
            {
                ExecuteMoveResponse response = client.ExecuteMove(body);

                // Fail if an HttpOperationException is not thrown
                Assert.IsTrue(false);
            }
            catch (HttpOperationException error)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, error.Response.StatusCode);
                Assert.IsTrue(error.Response.Content.Equals("\"gameboard must have a length of 9 (0 - 8)\""));
            }
        }

        [TestMethod]
        public void VerifyGameBoardImbalance()
        {
            MessagePayload body = new MessagePayload()
            {
                Move = 0,
                AzurePlayerSymbol = "O",
                HumanPlayerSymbol = "X",
                GameBoard = new string[9] {
                    "X", "?", "?",
                    "O", "?", "?",
                    "X", "X", "?",
                }
            };

            try
            {
                ExecuteMoveResponse response = client.ExecuteMove(body);

                // Fail if an HttpOperationException is not thrown
                Assert.IsTrue(false);
            }
            catch (HttpOperationException error)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, error.Response.StatusCode);
                Assert.IsTrue(error.Response.Content.Equals("\"The difference in symbol counts should be no greater than 1\""));
            }
        }

        [TestMethod]
        public void VerifyDoubleTurn()
        {
            MessagePayload body = new MessagePayload()
            {
                Move = 0,
                AzurePlayerSymbol = "O",
                HumanPlayerSymbol = "X",
                GameBoard = new string[9] {
                    "O", "?", "X",
                    "X", "?", "?",
                    "O", "O", "?"
                }
            };

            try
            {
                ExecuteMoveResponse response = client.ExecuteMove(body);

                // Fail if an HttpOperationException is not thrown
                Assert.IsTrue(false);
            }
            catch (HttpOperationException error)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, error.Response.StatusCode);
                Assert.IsTrue(error.Response.Content.Equals("\"There should be less AzurePlayerSymbols than humanPlayerSymbols on the gameBoard\""));
            }
        }

        [TestMethod]
        public void VerifyEmptyBoard()
        {
            MessagePayload body = new MessagePayload()
            {
                Move = 0,
                AzurePlayerSymbol = "O",
                HumanPlayerSymbol = "X",
                GameBoard = new string[9] {
                    "?", "?", "?",
                    "?", "?", "?",
                    "?", "?", "?"
                }
            };

            ExecuteMoveResponse response = client.ExecuteMove(body);

            // Verify that Azure took the first move
            var azurePlayerSymbolCount = response.GameBoard
                    .Select((value, index) => new { value, index })
                    .Where((tile) => tile.value.Equals(body.AzurePlayerSymbol))
                    .Select((tile) => tile.index)
                    .ToArray()
                    .Length;

            Assert.AreEqual(1, azurePlayerSymbolCount);
        }
    }
}
