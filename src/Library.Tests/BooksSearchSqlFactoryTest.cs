using FluentAssertions;
using Library.Infrastructure.Database.Search;
using Newtonsoft.Json;

namespace Library.Tests
{
    public class BooksSearchSqlFactoryTest
    {
        [Fact]
        public void GivenCreateSql_WhenBooksSearchFilterFilter_ThenBildCorrespondingSql()
        {
            var exp = JsonConvert.DeserializeObject<BooksSearchFilter>(@"
            {
                ""Operator"": ""AND"",
                ""Left"": {
                    ""Operator"": ""CONTAINS"",
                    ""Condition"": {
                        ""PropertyName"": ""Author"",
                        ""Value"": ""AuthorA""
                    }
                },
                ""Right"": {
                    ""Operator"": ""OR"",
                    ""Left"": {
                        ""Operator"": ""EQUAL"",
                        ""Condition"": {
                            ""PropertyName"": ""HoldingUserId"",
                            ""Value"": ""HolderC""
                        }
                    },
                    ""Right"": {
                        ""Operator"": ""CONTAINS"",
                        ""Condition"": {
                            ""PropertyName"": ""Description"",
                            ""Value"": ""DescriptionB""
                        }
                    }
                }
            }");

            var factory = new BooksSearchSqlFactory(exp);

            var sql = factory.CreateSql();

            sql.Should().Be("SELECT * FROM dbo.Book WHERE (Author LIKE '%AuthorA%')AND((HoldingUserId = 'HolderC')OR(Description LIKE '%DescriptionB%'))");
        }
}}