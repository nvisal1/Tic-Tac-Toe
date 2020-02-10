﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace ClientRestSDKLibrary.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;

    /// <summary>
    /// Defines the expected payload for executemove
    /// </summary>
    public partial class MessagePayload
    {
        /// <summary>
        /// Initializes a new instance of the MessagePayload class.
        /// </summary>
        public MessagePayload() { }

        /// <summary>
        /// Initializes a new instance of the MessagePayload class.
        /// </summary>
        public MessagePayload(int? move = default(int?), string azurePlayerSymbol = default(string), string humanPlayerSymbol = default(string), IList<string> gameBoard = default(IList<string>))
        {
            Move = move;
            AzurePlayerSymbol = azurePlayerSymbol;
            HumanPlayerSymbol = humanPlayerSymbol;
            GameBoard = gameBoard;
        }

        /// <summary>
        /// Indicates the position the human chooses.
        /// </summary>
        [JsonProperty(PropertyName = "move")]
        public int? Move { get; set; }

        /// <summary>
        /// Is either the letter X or the letter O indicating the
        /// symbol used by Azure.
        /// Must be the opposite of humanPlayerSymbol
        /// </summary>
        [JsonProperty(PropertyName = "azurePlayerSymbol")]
        public string AzurePlayerSymbol { get; set; }

        /// <summary>
        /// Is either the letter X or the letter O indicating the
        /// symbol used by the human.
        /// Must be the opposite of azurePlayerSymbol
        /// </summary>
        [JsonProperty(PropertyName = "humanPlayerSymbol")]
        public string HumanPlayerSymbol { get; set; }

        /// <summary>
        /// ? = A space on the game board
        /// X = Player letter X has a piece on that game board location
        /// O = Player letter O has a piece on that game board location
        /// </summary>
        [JsonProperty(PropertyName = "gameBoard")]
        public IList<string> GameBoard { get; set; }

    }
}
