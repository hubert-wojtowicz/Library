using System.Text;

namespace Library.Infrastructure.Database.Search;

public class BooksSearchSqlFactory
{
    private StringBuilder _sb;

    public string CreateSql() => $"SELECT * FROM dbo.Book WHERE {_sb}";

    public BooksSearchSqlFactory(BooksSearchFilter node)
    {
        _sb = new StringBuilder();
        ParseBookSearchExpression(node);
    }

    public void ParseBookSearchExpression(BooksSearchFilter node)
    {
        if (!IsAllowedOperator(node.Operator))
            throw new ArgumentException("Operator is not supported!");

        if (node.Condition == null)
        {
            if (node.Left != null)
            {
                _sb.Append("(");
                ParseBookSearchExpression(node.Left);
                _sb.Append(")");
            }

            if (node.Right != null)
            {
                _sb.Append(node.Operator).Append("(");
                ParseBookSearchExpression(node.Right);
                _sb.Append(")");
            }
        }
        else // (node.Condition != null)
        {
            // This is a leaf node, create a simple condition
            var value = node.Condition.Value;

            switch (node.Operator)
            {
                case "EQUAL":
                    _sb.Append(node.Condition.PropertyName).Append(" = '").Append(value).Append("'");
                    break;
                case "CONTAINS":
                    _sb.Append(node.Condition.PropertyName).Append(" LIKE '%").Append(value).Append("%'");
                    break;
            }
        }
    }

    private static readonly HashSet<string> Operators = new() { "AND", "OR", "CONTAINS", "EQUAL" };

    private bool IsAllowedOperator(string nodeOperator)
    {
        return Operators.Contains(nodeOperator);
    }
}