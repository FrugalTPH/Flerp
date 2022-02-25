using DevExpress.XtraEditors.Repository;
using Flerp.Client.Properties;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Flerp.Client.Helpers
{
    public class ColumnDefinition
    {
        public string PropertyName { get; private set; }
        public RepositoryItem Ri { get; private set; }
        public bool IsHidden { get; private set; }
        public bool IsReadOnly { get; private set; }
        public string Caption { get; private set; }
        public string ToolTip { get; private set; }
        public int MinWidth { get; private set; }
        public int MaxWidth { get; private set; }


        public static ColumnDefinition Create<T>(Expression<Func<T, object>> propertyRefExpr, RepositoryItem ri, 
            bool isHidden, bool isReadOnly, int minWidth = -1, int maxWidth = -1, string caption = null, string tooltip = null)
        {
            return new ColumnDefinition
            {
                PropertyName = GetPropertyNameCore(propertyRefExpr.Body),
                Ri = ri,
                IsHidden = isHidden,
                IsReadOnly = isReadOnly,
                Caption = caption,
                ToolTip = tooltip,
                MinWidth = minWidth,
                MaxWidth = maxWidth,             
            };
        }

        private static string GetPropertyNameCore(Expression propertyRefExpr)
        {
            if (propertyRefExpr == null) throw new ArgumentNullException(Resources.Name_propertyRefExpr, Resources.Msg_propertyRefExprIsNull);

            var memberExpr = propertyRefExpr as MemberExpression;
            if (memberExpr == null)
            {
                var unaryExpr = propertyRefExpr as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                    memberExpr = unaryExpr.Operand as MemberExpression;
            }

            if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property) return memberExpr.Member.Name;

            throw new ArgumentException(Resources.Msg_NoPropertyRefExpFound, Resources.Name_propertyRefExpr);
        }
    }
}
