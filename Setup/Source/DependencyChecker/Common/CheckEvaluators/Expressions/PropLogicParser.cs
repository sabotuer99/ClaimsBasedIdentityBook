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
    using System.Text;

    public static class PropLogicParser
    {
        public static PropExpression Parse(string source)
        {
            var lexer = new PropLogicLexer(source);
            lexer.NextToken();
            PropExpression result = MatchExpression(lexer);
            ExpectToken(lexer, LexerTokenType.End);
            return result;
        }

        private static void ExpectToken(PropLogicLexer lexer, params LexerTokenType[] tokens)
        {
            foreach (LexerTokenType token in tokens)
            {
                if (lexer.CurrentTokenType == token)
                {
                    return;
                }
            }

            var sb = new StringBuilder();
            foreach (LexerTokenType token in tokens)
            {
                if (sb.Length > 0)
                {
                    sb.Append(", ");
                }
                sb.Append(TokenTypeToString(token));

                throw new ArgumentException(string.Format("Unexpected token in expression, got {0}, expecting one of {1}", TokenTypeToString(lexer.CurrentTokenType), sb));
            }
        }

        private static PropExpression MatchAndTerm(PropLogicLexer lexer)
        {
            PropExpression result = MatchUnary(lexer);
            while (lexer.CurrentTokenType == LexerTokenType.And)
            {
                lexer.NextToken();
                PropExpression right = MatchUnary(lexer);
                result = new PropConjunction(result, right);
            }
            return result;
        }

        private static PropExpression MatchAtom(PropLogicLexer lexer)
        {
            ExpectToken(lexer, LexerTokenType.Identifier, LexerTokenType.OpenParen);

            PropExpression result;
            if (lexer.CurrentTokenType == LexerTokenType.Identifier)
            {
                result = new PropIdentifier(lexer.CurrentTokenValue);
                lexer.NextToken();
            }
            else
            {
                lexer.NextToken();
                result = MatchExpression(lexer);
                ExpectToken(lexer, LexerTokenType.CloseParen);
                lexer.NextToken();
            }
            return result;
        }

        // BNF(ish) for the language this parses
        // Expression ::== OrTerm
        // OrTerm ::== AndTerm ( '||' AndTerm)*
        // AndTerm ::== Unary ( '&&' Unary)*
        // Unary ::= '!' Atom | Atom
        // Atom ::= IDENTIFIER | '(' Expression ')'
        // IDENTIFIER ::= [A-Za-z_][A-Za-z0-9_]*
        private static PropExpression MatchExpression(PropLogicLexer lexer)
        {
            PropExpression result = MatchOrTerm(lexer);
            return result;
        }

        private static PropExpression MatchOrTerm(PropLogicLexer lexer)
        {
            PropExpression result = MatchAndTerm(lexer);
            while (lexer.CurrentTokenType == LexerTokenType.Or)
            {
                lexer.NextToken();
                PropExpression right = MatchAndTerm(lexer);
                result = new PropDisjunction(result, right);
            }
            return result;
        }

        private static PropExpression MatchUnary(PropLogicLexer lexer)
        {
            if (lexer.CurrentTokenType == LexerTokenType.Not)
            {
                lexer.NextToken();
                PropExpression result = MatchAtom(lexer);
                return new PropNegation(result);
            }
            return MatchAtom(lexer);
        }

        private static string TokenTypeToString(LexerTokenType token)
        {
            switch (token)
            {
                case LexerTokenType.NotStarted:
                    return "Before first token";
                case LexerTokenType.Identifier:
                    return "identifier";
                case LexerTokenType.And:
                    return "&&";
                case LexerTokenType.Or:
                    return "||";
                case LexerTokenType.Not:
                    return "!";
                case LexerTokenType.OpenParen:
                    return "(";
                case LexerTokenType.CloseParen:
                    return ")";
                case LexerTokenType.End:
                    return "End of expression";
                default:
                    return "Can't get here, shut up the compiler";
            }
        }
    }
}