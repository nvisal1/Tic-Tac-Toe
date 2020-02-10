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

        [TestMethod]
        public void VerifyPlayerXWinner()
        {
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials("FakeTokenValue");

            ClientRestSDKLibraryClient client = new ClientRestSDKLibraryClient(new Uri(LocalEndpointUrl), serviceClientCredentials);

            MessagePayload body = new MessagePayload()
            {
                Move = 0,
                AzurePlayerSymbol = "X",
                HumanPlayerSymbol = "O",
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
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials("FakeTokenValue");

            ClientRestSDKLibraryClient client = new ClientRestSDKLibraryClient(new Uri(LocalEndpointUrl), serviceClientCredentials);

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
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials("FakeTokenValue");

            ClientRestSDKLibraryClient client = new ClientRestSDKLibraryClient(new Uri(LocalEndpointUrl), serviceClientCredentials);

            MessagePayload body = new MessagePayload()
            {
                Move = 0,
                AzurePlayerSymbol = "X",
                HumanPlayerSymbol = "O",
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
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials("FakeTokenValue");

            ClientRestSDKLibraryClient client = new ClientRestSDKLibraryClient(new Uri(LocalEndpointUrl), serviceClientCredentials);

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
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials("FakeTokenValue");

            ClientRestSDKLibraryClient client = new ClientRestSDKLibraryClient(new Uri(LocalEndpointUrl), serviceClientCredentials);

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
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials("FakeTokenValue");

            ClientRestSDKLibraryClient client = new ClientRestSDKLibraryClient(new Uri(LocalEndpointUrl), serviceClientCredentials);

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
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials("FakeTokenValue");

            ClientRestSDKLibraryClient client = new ClientRestSDKLibraryClient(new Uri(LocalEndpointUrl), serviceClientCredentials);

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
            }
        }

        [TestMethod]
        public void VerifyInvalidPlayerSymbol()
        {
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials("FakeTokenValue");

            ClientRestSDKLibraryClient client = new ClientRestSDKLibraryClient(new Uri(LocalEndpointUrl), serviceClientCredentials);

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
            }
        }

        [TestMethod]
        public void VerifyInvalidGameBoardSymbols()
        {
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials("FakeTokenValue");

            ClientRestSDKLibraryClient client = new ClientRestSDKLibraryClient(new Uri(LocalEndpointUrl), serviceClientCredentials);

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
            }
        }

        [TestMethod]
        public void VerifySmallerGameBoardLength()
        {
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials("FakeTokenValue");

            ClientRestSDKLibraryClient client = new ClientRestSDKLibraryClient(new Uri(LocalEndpointUrl), serviceClientCredentials);

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
            }
        }

        [TestMethod]
        public void VerifyLargerGameBoardLength()
        {
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials("FakeTokenValue");

            ClientRestSDKLibraryClient client = new ClientRestSDKLibraryClient(new Uri(LocalEndpointUrl), serviceClientCredentials);

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
            }
        }

        [TestMethod]
        public void VerifyGameBoardImbalance()
        {
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials("FakeTokenValue");

            ClientRestSDKLibraryClient client = new ClientRestSDKLibraryClient(new Uri(LocalEndpointUrl), serviceClientCredentials);

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
            }
        }

        [TestMethod]
        public void VerifyDoubleTurn()
        {
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials("FakeTokenValue");

            ClientRestSDKLibraryClient client = new ClientRestSDKLibraryClient(new Uri(LocalEndpointUrl), serviceClientCredentials);

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
            }
        }

        [TestMethod]
        public void VerifyEmptyBoard()
        {
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials("FakeTokenValue");

            ClientRestSDKLibraryClient client = new ClientRestSDKLibraryClient(new Uri(LocalEndpointUrl), serviceClientCredentials);

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
