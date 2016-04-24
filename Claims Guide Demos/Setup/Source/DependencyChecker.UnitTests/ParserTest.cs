//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace DependencyChecker.UnitTests
{
    using System;
    using DependencyChecker.Common.CheckEvaluators.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void CanParseIdentifier()
        {
            PropExpression e = PropLogicParser.Parse("MyIden");

            Assert.IsInstanceOfType(e, typeof(PropIdentifier));
            Assert.AreEqual("MyIden", ((PropIdentifier)e).Name);
        }

        [TestMethod]
        public void CanParseIdentifierWithNumber()
        {
            PropExpression e = PropLogicParser.Parse("MyIden2");

            Assert.IsInstanceOfType(e, typeof(PropIdentifier));
            Assert.AreEqual("MyIden2", ((PropIdentifier)e).Name);
        }

        [TestMethod]
        public void CanNegate()
        {
            PropExpression e = PropLogicParser.Parse("!Term");
            Assert.IsInstanceOfType(e, typeof(PropNegation));
            
            var en = (PropNegation)e;

            Assert.IsInstanceOfType(en.Inner, typeof(PropIdentifier));
            Assert.AreEqual("Term", ((PropIdentifier)en.Inner).Name);
        }

        [TestMethod]
        public void CanDoConjunction()
        {
            PropExpression e = PropLogicParser.Parse("A && B");
            Assert.IsInstanceOfType(e, typeof(PropConjunction));
            
            var ec = (PropConjunction)e;
            Assert.IsInstanceOfType(ec.Left, typeof(PropIdentifier));
            Assert.IsInstanceOfType(ec.Right, typeof(PropIdentifier));
            Assert.AreEqual("A", ((PropIdentifier)ec.Left).Name);
            Assert.AreEqual("B", ((PropIdentifier)ec.Right).Name);
        }

        [TestMethod]
        public void CanDoDisjunction()
        {
            PropExpression e = PropLogicParser.Parse("A || B");
            Assert.IsInstanceOfType(e, typeof(PropDisjunction));

            var ec = (PropDisjunction)e;
            Assert.IsInstanceOfType(ec.Left, typeof(PropIdentifier));
            Assert.IsInstanceOfType(ec.Right, typeof(PropIdentifier));
            Assert.AreEqual("A", ((PropIdentifier)ec.Left).Name);
            Assert.AreEqual("B", ((PropIdentifier)ec.Right).Name);
        }

        [TestMethod]
        public void CanParenthesisIdentifier()
        {
            PropExpression e = PropLogicParser.Parse("(B)");
            
            Assert.IsInstanceOfType(e, typeof(PropIdentifier));
            Assert.AreEqual("B", ((PropIdentifier)e).Name);
        }

        [TestMethod]
        public void CanParseWithMultipleMatchedParentheses()
        {
            PropExpression e = PropLogicParser.Parse("(((Identifier)))");
            
            Assert.IsInstanceOfType(e, typeof(PropIdentifier));
            Assert.AreEqual("Identifier", ((PropIdentifier)e).Name);
        }

        [TestMethod]
        public void CanCombineNotWithParens()
        {
            PropExpression e = PropLogicParser.Parse("!(Identifier)");
            Assert.IsInstanceOfType(e, typeof(PropNegation));
            
            var neg = (PropNegation)e;
            Assert.IsInstanceOfType(neg.Inner, typeof(PropIdentifier));
            Assert.AreEqual("Identifier", ((PropIdentifier)neg.Inner).Name);
        }

        [TestMethod]
        public void CanDoComplexExpression()
        {
            PropExpression e = PropLogicParser.Parse("(A || !B) && C");
            Assert.IsInstanceOfType(e, typeof(PropConjunction));
            
            var ec = (PropConjunction)e;
            Assert.IsInstanceOfType(ec.Left, typeof(PropDisjunction));
            Assert.IsInstanceOfType(ec.Right, typeof(PropIdentifier));
            Assert.AreEqual("C", ((PropIdentifier)ec.Right).Name);

            var ed = (PropDisjunction)ec.Left;
            Assert.IsInstanceOfType(ed.Left, typeof(PropIdentifier));
            Assert.IsInstanceOfType(ed.Right, typeof(PropNegation));
            Assert.AreEqual("A", ((PropIdentifier)ed.Left).Name);

            var en = (PropNegation)ed.Right;
            Assert.IsInstanceOfType(en.Inner, typeof(PropIdentifier));
            Assert.AreEqual("B", ((PropIdentifier)en.Inner).Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WillThrowForInvalidExpression()
        {
            PropLogicParser.Parse("++%$");
        }
    }
}