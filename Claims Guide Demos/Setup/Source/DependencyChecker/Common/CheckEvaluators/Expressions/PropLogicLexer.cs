//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace DependencyChecker.Common.CheckEvaluators.Expressions
{
    using System;
    using System.Text.RegularExpressions;

    internal enum LexerTokenType
    {
        NotStarted = 0, 
        Identifier, 
        And, 
        Or, 
        Not, 
        OpenParen, 
        CloseParen, 
        End
    }

    internal class PropLogicLexer
    {
        private readonly Regex identifierRegex;
        private LexerTokenType currentTokenType;
        private string currentTokenValue;

        private string source;

        public PropLogicLexer(string source)
        {
            this.source = source;
            this.currentTokenType = LexerTokenType.NotStarted;
            this.currentTokenValue = null;
            this.identifierRegex = new Regex("^[A-Za-z_][0-9A-Za-z_]*");
        }

        public LexerTokenType CurrentTokenType
        {
            get { return this.currentTokenType; }
        }

        public string CurrentTokenValue
        {
            get { return this.currentTokenValue; }
        }

        public bool NextToken()
        {
            if (this.currentTokenType == LexerTokenType.End)
            {
                return false;
            }
            this.EatLeadingWhiteSpace();
            if (this.source.Length == 0)
            {
                this.currentTokenType = LexerTokenType.End;
                this.currentTokenValue = string.Empty;
                return false;
            }

            if (this.MatchesKeyword("&&", LexerTokenType.And))
            {
                return true;
            }
            if (this.MatchesKeyword("||", LexerTokenType.Or))
            {
                return true;
            }
            if (this.MatchesKeyword("!", LexerTokenType.Not))
            {
                return true;
            }
            if (this.MatchesKeyword("(", LexerTokenType.OpenParen))
            {
                return true;
            }
            if (this.MatchesKeyword(")", LexerTokenType.CloseParen))
            {
                return true;
            }
            if (this.MatchesIdentifier())
            {
                return true;
            }

            throw new ArgumentException("Invalid input stream in source");
        }

        private void EatLeadingWhiteSpace()
        {
            this.source = this.source.TrimStart(new[] { ' ', '\t' });
        }

        private bool MatchesIdentifier()
        {
            Match match = this.identifierRegex.Match(this.source);
            if (match.Success)
            {
                this.currentTokenType = LexerTokenType.Identifier;
                this.currentTokenValue = this.source.Substring(0, match.Length);
                this.source = this.source.Substring(match.Length);
                return true;
            }
            return false;
        }

        private bool MatchesKeyword(string keyword, LexerTokenType tokenType)
        {
            if (this.source.StartsWith(keyword))
            {
                this.currentTokenType = tokenType;
                this.currentTokenValue = keyword;
                this.source = this.source.Substring(keyword.Length);
                return true;
            }
            return false;
        }
    }
}