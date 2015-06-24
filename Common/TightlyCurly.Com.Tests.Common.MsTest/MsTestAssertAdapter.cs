using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Tests.Common.Helpers;

namespace TightlyCurly.Com.Tests.Common.MsTest
{
    public class MsTestAssertAdapter : IAssertAdapter
    {
        public void AreEqual(object expected, object actual, string message = null)
        {
            DoAssert(() =>
            {
                if (message.IsNull())
                {
                    Assert.AreEqual(expected, actual);
                }
                else
                {
                    Assert.AreEqual(expected, actual, message);
                }
            });
        }

        public void IsTrue(bool condition, string message = null)
        {
            DoAssert(() =>
                {
                    if (message.IsNull())
                    {
                        Assert.IsTrue(condition);
                    }
                    else
                    {
                        Assert.IsTrue(condition, message);
                    }
                });
        }

        public void AreEqual<TItem>(TItem expected, TItem actual, 
            string message = null)
        {
            DoAssert(() =>
                {
                    if (message.IsNull())
                    {
                        Assert.AreEqual(expected, actual);
                    }
                    else
                    {
                        Assert.AreEqual(expected, actual, message);
                    }
                });
        }

        public void AreNotEqual<TItem>(TItem expected, TItem actual, string message = null)
        {
            DoAssert(() =>
                {
                    if (message.IsNull())
                    {
                        Assert.AreNotEqual(expected, actual);
                    }
                    else
                    {
                        Assert.AreNotEqual(expected, actual, message);
                    }
                });
        }

        public void IsFalse(bool condition, string message = null)
        {
            DoAssert(() =>
                {
                    if (message.IsNull())
                    {
                        Assert.IsFalse(condition);
                    }
                    else
                    {
                        Assert.IsFalse(condition, message);
                    }
                });
        }

        public void Inconclusive(string message)
        {
            DoAssert(() =>
            {
                Assert.Inconclusive(message);
            });
        }

        public void IsNotNull(object value, string message = null)
        {
            DoAssert(() =>
            {
                if (message.IsNull())
                {
                    Assert.IsNotNull(value);
                }
                else
                {
                    Assert.IsNotNull(value, message);
                }
            });
        }

        public void IsNull(object value, string message = null)
        {
            DoAssert(() =>
            {
                if (message.IsNull())
                {
                    Assert.IsNull(value);
                }
                else
                {
                    Assert.IsNull(value, message);
                }
            });
        }

        private void DoAssert(Action action)
        {
            try
            {
                action();
            }
            catch (AssertFailedException exception)
            {
                throw new AssertException(exception.Message, exception);
            }
        }
    }
}
