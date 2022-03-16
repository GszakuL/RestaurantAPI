**BEFORE YOU START**  
Init your local database for project with automatic data seed configured in project.  
In RestaurantDAL/Entities/RestaurantDbContext.cs you can set your local database connection string in private variable __connectionString_.   
1. Set RestaurantDAL as Startup Project.
2. Go to Package Manager Console, change Default Project to RestaurantDAL.
3. To create migration use in console following command: add-migration 'YourMigrationName'.
4. Save database changes using command: update-database.
5. Set back RestaurantAPI as Startup Project.
6. Now you can run project, initial data should be seeded in database.
