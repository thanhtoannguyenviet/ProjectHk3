using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DemoQuanTrong.Models;

namespace DemoQuanTrong.Common
{
    public class CustomSQL
    {
        private static string defaultSelect()
        {
            return "SELECT A.* FROM $TABLE$ A";
        }

        private static string defaultCount()
        {
            return "SELECT COUNT(A.*) FROM $TABLE$ A"; //nếu thêm where thì đuôi add thêm $ANDWHERE
        }
        public static string QuerySelect(string tableName, Filter filter = null)
        {

            string query = defaultSelect();
            if (tableName == null)
            {
                throw new Exception();
            }
            return query.Replace("$TABLE$", tableName);

        }

        public static string QueryCount(string tableName, Filter filter = null)
        {

            string query = defaultCount();
            if (tableName == null)
            {
                throw new Exception();
            }
            return query.Replace("$TABLE$", tableName);

        }
        private static string pagination(string query, Filter filter)
        {
            int pageNumber = 0;
            int pageSize = 20;
            query.Replace("$ANDWHERE", " ");
            if (filter == null)
            {
                query += " ORDER BY id";
                int getRow = pageNumber * pageSize;
                query += " OFFSET " + getRow + " ROWS ";
                query += " FETCH NEXT " + pageSize + " ROWS ONLY ";
            }
            else
            {


                query += " ORDER BY " + filter.conditionOrderBy;
                if (!filter.orderBy)
                {
                    query += " DESC ";
                }

                pageNumber = filter.pageNumber;
                pageSize = filter.pageSize;
                int getRow = pageNumber * pageSize;
                query += " OFFSET " + getRow + " ROWS ";
                query += " FETCH NEXT " + pageSize + " ROWS ONLY ";
            }
            return query;
        }

        private static string SQLSearch(string query, string keyword, string columnSearch)
        {
            if (!"".Equals(keyword))
            {
                if (query.Contains("$ANDWHERE"))
                {
                    query.Replace("$ANDWHERE", " AND " + columnSearch + " LIKE = %'" + keyword + "'% ");
                }
                else
                {
                    query += " WHERE " + columnSearch + " LIKE = %'" + keyword + "'%  $ANDWHERE ";
                }
            }
            return query;

        }
        private static string formatCondition(string condition)
        {
            return "'" + condition + "'";
        }

        private static string SQLWhere(string query, string column, string condition, bool stringColumn = false)
        {
            if (stringColumn)
            {
                condition = formatCondition(condition);
            }
            if (query.Contains("$ANDWHERE"))
                return query.Replace("$ANDWHERE", " AND " + column + " = " + condition + " $ANDWHERE ");
            else
                return query += " WHERE " + column + " = " + condition + " $ANDWHERE ";
        }
        private static string SQLLessWhere(string query, string column, string condition, bool stringColumn = false)
        {
            if (stringColumn)
            {
                condition = formatCondition(condition);
            }
            if (query.Contains("$ANDWHERE"))
                return query.Replace("$ANDWHERE", " AND " + column + " < " + condition + " $ANDWHERE ");
            else
                return query += " WHERE " + column + " < " + condition + " $ANDWHERE ";
        }
        public static string clear(string query)
        {
            if (query.Contains("$ANDWHERE"))
                return query.Replace("$ANDWHERE", "");
            return query;
        }
        public static string getStaffForCustomer(Filter filter, Boolean count = false)
        {
            string query = CustomSQL.QuerySelect(ConstantTable.STAFF);
            if (count)
            {
                query = CustomSQL.QueryCount(ConstantTable.STAFF);
            }
            query += " INNER JOIN Account B ON A.id = B.id ";
            query = CustomSQL.SQLLessWhere(query, "role_", 25 + "");
            if (filter != null)
                query = CustomSQL.SQLSearch(query, filter.keyword, "staffName");
            if (!count)
            {
                query = CustomSQL.pagination(query, filter);
            }
            return clear(query); ;
        }

        public static string findStaff(Filter filter = null, int id = 0)
        {
            string query = CustomSQL.QuerySelect(ConstantTable.STAFF);
            query += " INNER JOIN Account B ON A.id = B.id ";
            query = CustomSQL.SQLLessWhere(query, "role_", 25 + "");
            if (filter != null)
                query = CustomSQL.SQLSearch(query, filter.keyword, "staffName");
            if (id != 0)
            {
                query = CustomSQL.SQLWhere(query, "id", id + "");
            }
            query = CustomSQL.pagination(query, filter);
            return clear(query); ;
        }

        public static string checkLogin(string userName, string password)
        {
            string query = CustomSQL.QuerySelect(ConstantTable.ACCOUNT);
            query = CustomSQL.SQLWhere(query, "userName", userName, true);
            query = CustomSQL.SQLWhere(query, "pass_word", password, true);
            return clear(query); ;
        }
        public static string checkRole(string tableName, string id)
        {
            string query = "";
            query = CustomSQL.QuerySelect(tableName);
            query = CustomSQL.SQLWhere(query, "id", id);
            return clear(query); ;
        }

        public static string getImg(string entryName, string entryId)
        {
            string query = CustomSQL.QuerySelect(ConstantTable.IMG);
            query = CustomSQL.SQLWhere(query, "entryName", entryName, true);
            query = CustomSQL.SQLWhere(query, "entryId", entryId);
            return clear(query); ;
        }

        public static string findForCustomer(string tableName, string id, Filter filter, Boolean count = false)
        {
            string query = CustomSQL.QuerySelect(tableName);
            query = CustomSQL.SQLWhere(query, "customerId", id);
            if (!count)
            {
                query = CustomSQL.pagination(query, filter);
            }
            return clear(query); ;
        }

        public static string getService(int id)
        {
            string query = CustomSQL.QuerySelect(ConstantTable.SERVICE);
            query = CustomSQL.SQLWhere(query, "staffId", id + "");
            return clear(query); ;
        }


    }


}