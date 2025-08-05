using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class Added_GetProducts_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql
            (
                @"
						CREATE OR ALTER PROCEDURE GetProducts
							@PageNumber INT,
							@PageSize INT,
							@SortBy NVARCHAR(50),
							@SearchTerm NVARCHAR(50),
							@TotalCount INT OUTPUT
						AS

							BEGIN 
								Select @TotalCount = Count(*) from Products
								WHERE Name LIKE '%' + @SearchTerm + '%'

								-- @SearchTerm=Iphone  ==>    like '%Iphone%'

								Select Id, Name, Price
								FROM Products
								Where Name LIKE '%' + @SearchTerm + '%'   -- Filtering
								ORDER BY                                  -- Sorting
									CASE 
										When @SortBy = 'Name' Then Name
										ELSE NULL
									END,
									CASE 
										When @SortBy = 'Price' Then Price
										ELSE NULL
									END
								OFFSET (@PageNumber - 1) * @PageSize ROWS  -- Pagination
								FETCH NEXT @PageSize ROWS ONLY;

							END
                    "
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE GetProducts");
        }
    }
}
