using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Identity;
using Lab1.Pages.DataClasses;
using Lab1.Pages.Data_Classes;
using System.Data;
using Lab1.Pages.Admin;
using Microsoft.VisualBasic;


namespace Lab1.Pages.DB
{
    public class DBClass
    {
        public string UserID { get; set; }


        // Use this class to define methods that make connecting to
        // and retrieving data from the DB easier.

        // Connection Object at Data Field Level
        public static SqlConnection Lab1DBConnection = new SqlConnection();

        // Connection String - How to find and connect to DB
        private static readonly String? Lab1DBConnString =
            "Server=localhost;Database=JMUCare;Trusted_Connection=True";

        private static readonly String? AuthConnString =
            "Server=Localhost;Database=AUTH;Trusted_Connection=True";




        public static int AddUser(User newUser, int CurrentUserID) //Updated for JMU Care DB
        {
            SqlCommand cmdAddUser = new SqlCommand();
            cmdAddUser.Connection = Lab1DBConnection;
            cmdAddUser.Connection.ConnectionString = Lab1DBConnString;
            cmdAddUser.CommandType = CommandType.StoredProcedure;
            cmdAddUser.CommandText = "InsertUserAndGetID";

            // Add input parameters
            cmdAddUser.Parameters.AddWithValue("@FirstName", newUser.FirstName);
            cmdAddUser.Parameters.AddWithValue("@LastName", newUser.LastName);
            cmdAddUser.Parameters.AddWithValue("@Email", newUser.Email);
            cmdAddUser.Parameters.AddWithValue("@UserTypeID", newUser.UserTypeID);
            cmdAddUser.Parameters.AddWithValue("@Department", newUser.Department);
            cmdAddUser.Parameters.AddWithValue("@DateJoined", newUser.DateJoined);
            cmdAddUser.Parameters.AddWithValue("@AccessLevel", newUser.AccessLevel);

            // Add output parameter for UserID
            SqlParameter outputIdParam = new SqlParameter("@NewUserID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmdAddUser.Parameters.Add(outputIdParam);

            cmdAddUser.Connection.Open();

            int rowsAffected = cmdAddUser.ExecuteNonQuery(); // Execute procedure
            int newUserId = -1;

            if (rowsAffected > 0)
            {
                newUserId = (int)outputIdParam.Value;
                Console.WriteLine($"User inserted successfully. New User ID: {newUserId}");
            }
            else
            {
                Console.WriteLine("No rows were inserted.");
            }

            cmdAddUser.Connection.Close();

            return newUserId;
        }

        public static SqlDataReader ViewAllGrants()
        {
            string GrantSelectString = "SELECT * FROM Grants;";
            SqlCommand cmdViewGrants = new SqlCommand();
            cmdViewGrants.Connection = Lab1DBConnection;
            cmdViewGrants.Connection.ConnectionString = Lab1DBConnString;
            cmdViewGrants.CommandText = GrantSelectString;
            Lab1DBConnection.Open();

            SqlDataReader tempReader = cmdViewGrants.ExecuteReader();

            return tempReader;
        } //Ready for JMU Care DB

        public static SqlDataReader ViewUserMessages(int UserID)
        {
            string MessageSelectString = "SELECT Content FROM Message WHERE senderID = @UserID;";
            SqlCommand cmdViewMessage = new SqlCommand();
            cmdViewMessage.Connection = Lab1DBConnection;
            cmdViewMessage.Connection.ConnectionString = Lab1DBConnString;
            cmdViewMessage.CommandText = MessageSelectString;

            cmdViewMessage.Parameters.AddWithValue("@UserID", UserID);

            Lab1DBConnection.Open();

            SqlDataReader tempreader = cmdViewMessage.ExecuteReader();
            return tempreader;
        }

        public static void AddGrant(Grant NewGrant) //Updated for JMU Care DB
        {
            string AddGrantString = @"
            INSERT INTO Grant
            (GrantName, FundingAgency, SubmissionDate, Deadline, ProposalID, FundingAmount, Type, GrantDescription, UserID)
            VALUES
            (@GrantName, @FundingAgency, @SubmissionDate, @Deadline, @ProposalID, @FundingAmount, @Type, @GrantDescription)";
            SqlCommand cmdAddGrant = new SqlCommand();
            cmdAddGrant.Connection = Lab1DBConnection;
            cmdAddGrant.Connection.ConnectionString = Lab1DBConnString;
            cmdAddGrant.CommandText = AddGrantString;

            cmdAddGrant.Parameters.Add("@GrantName", SqlDbType.VarChar, 100).Value = NewGrant.GrantName;
            cmdAddGrant.Parameters.Add("@FundingAgency", SqlDbType.VarChar, 100).Value = NewGrant.FundingAgency;
            cmdAddGrant.Parameters.Add("@Deadline", SqlDbType.Date).Value = NewGrant.Deadline;
            cmdAddGrant.Parameters.Add("@ProposalID", SqlDbType.Int).Value = NewGrant.ProposalID;
            cmdAddGrant.Parameters.Add("@FundingAmount", SqlDbType.Decimal).Value = NewGrant.FundingAmount;
            cmdAddGrant.Parameters.Add("@Type", SqlDbType.VarChar, 50).Value = NewGrant.Type;
            cmdAddGrant.Parameters.Add("@GrantDescription", SqlDbType.VarChar).Value = NewGrant.GrantDescription;



            Lab1DBConnection.Open();
            int rowsAffected = cmdAddGrant.ExecuteNonQuery(); // Ensures execution

            if (rowsAffected > 0)
            {
                Console.WriteLine("Data inserted successfully.");
            }
            else
            {
                Console.WriteLine("No rows were inserted.");
            }

            Lab1DBConnection.Close();
        }

        public static void AddBusinessPartner(BusinessPartner NewBusinessPartner) //Updated for JMU Care DB
        {
            string AddPartnerString = @"
            INSERT INTO Partnership
            (PartnerName, PartnerOrg, PartnerContact, PartnerType, Sector, Status, LastInteractionDate, GrantID)
            VALUES
            (@PartnerName, @PartnerOrg, @PartnerContact, @PartnerType, @Sector, @Status, @LastInteractionDate, @GrantID)";

            SqlCommand cmdAddPartner = new SqlCommand();
            cmdAddPartner.Connection = Lab1DBConnection;
            cmdAddPartner.Connection.ConnectionString = Lab1DBConnString;
            cmdAddPartner.CommandText = AddPartnerString;

            cmdAddPartner.Parameters.Add("@PartnerName", SqlDbType.VarChar, 100).Value = NewBusinessPartner.PartnerName;
            cmdAddPartner.Parameters.Add("@PartnerOrg", SqlDbType.VarChar, 100).Value = NewBusinessPartner.PartnerOrg;
            cmdAddPartner.Parameters.Add("@PartnerContact", SqlDbType.VarChar, 100).Value = NewBusinessPartner.PartnerContact;
            cmdAddPartner.Parameters.Add("@PartnerType", SqlDbType.VarChar, 50).Value = NewBusinessPartner.PartnerType;
            cmdAddPartner.Parameters.Add("@Sector", SqlDbType.VarChar, 50).Value = NewBusinessPartner.Sector;
            cmdAddPartner.Parameters.Add("@Status", SqlDbType.VarChar, 50).Value = NewBusinessPartner.Status;
            cmdAddPartner.Parameters.Add("@LastInteractionDate", SqlDbType.Date).Value = NewBusinessPartner.LastInteractionDate;
            cmdAddPartner.Parameters.Add("@GrantID", SqlDbType.Int).Value = NewBusinessPartner.GrantID;



            Lab1DBConnection.Open();


            int rowsAffected = cmdAddPartner.ExecuteNonQuery(); // Ensures execution

            if (rowsAffected > 0)
            {
                Console.WriteLine("Data inserted successfully.");
            }
            else
            {
                Console.WriteLine("No rows were inserted.");
            }




        }

        public static SqlDataReader ViewAllProjects()
        {
            string ViewAdminProjectsString = "SELECT Project.name, Grants.amount , Project.dueDate " +
                "FROM Project JOIN Grants ON Project.grantID = Grants.grantID ;";


            SqlCommand cmdViewAdminProjects = new SqlCommand();
            cmdViewAdminProjects.Connection = Lab1DBConnection;
            cmdViewAdminProjects.Connection.ConnectionString = Lab1DBConnString;
            cmdViewAdminProjects.CommandText = ViewAdminProjectsString;
            Lab1DBConnection.Open();

            SqlDataReader tempReader = cmdViewAdminProjects.ExecuteReader();
            return tempReader;

        }






        public static void AddNewProject(Project project, int currentUserID) //Method needs to also generate user-project pairs for composite table in DB
        {
            //string getFacultyString = "SELECT facultyID FROM FacultyGrant WHERE grantID = @grantID;";
            //SqlCommand cmdGetFaculty = new SqlCommand();
            //cmdGetFaculty.Connection = Lab1DBConnection;
            //cmdGetFaculty.Connection.ConnectionString = Lab1DBConnString;
            //cmdGetFaculty.CommandText = getFacultyString;

            //cmdGetFaculty.Parameters.AddWithValue("@grantID", project.GrantID);

            //Lab1DBConnection.Open();

            //SqlDataReader facultyReader = cmdGetFaculty.ExecuteReader();

            //int facultyResult = 0;
            //if (facultyReader.Read())
            //{
            //    facultyResult = Convert.ToInt32(facultyReader["facultyID"]);
            //}
            //Lab1DBConnection.Close();

            string sqlQuery = @"
    INSERT INTO Project
        (ProjectName, StartDate, EndDate, ProjectStatus, ProgressStatus, ProjectLead, UserID, GrantID)
    OUTPUT INSERTED.ProjectID
    VALUES
        (@ProjectName, @StartDate, @EndDate, @ProjectStatus, @ProgressStatus, @ProjectLead, @UserID, @GrantID)";

            SqlCommand cmdAddProject = new SqlCommand();
            cmdAddProject.Connection = Lab1DBConnection;
            cmdAddProject.Connection.ConnectionString = Lab1DBConnString;
            cmdAddProject.CommandText = sqlQuery;

            cmdAddProject.Parameters.AddWithValue("@ProjectName", project.ProjectName);
            cmdAddProject.Parameters.AddWithValue("@StartDate", project.StartDate);
            cmdAddProject.Parameters.AddWithValue("@EndDate", project.EndDate);
            cmdAddProject.Parameters.AddWithValue("@ProjectStatus", project.ProjectStatus);
            cmdAddProject.Parameters.AddWithValue("@ProgressStatus", project.ProgressStatus);
            cmdAddProject.Parameters.AddWithValue("@ProjectLead", project.ProjectLead);
            cmdAddProject.Parameters.AddWithValue("@UserID", project.UserID);
            cmdAddProject.Parameters.AddWithValue("@GrantID", project.GrantID);

            cmdAddProject.Connection.Open();
            int newProjectID = (int)cmdAddProject.ExecuteScalar();


            Lab1DBConnection.Close();

            string NoteInsertQuery = "INSERT INTO Note (ProjectID, note) VALUES (@ProjectID, @note);";
            SqlCommand cmdAddNote = new SqlCommand();
            cmdAddNote.Connection = Lab1DBConnection;
            cmdAddNote.Connection.ConnectionString = Lab1DBConnString;
            cmdAddNote.CommandText = NoteInsertQuery;

            cmdAddNote.Parameters.AddWithValue("@ProjectID", newProjectID);
            //cmdAddNote.Parameters.AddWithValue("@note", project.note);

            Lab1DBConnection.Open();
            cmdAddNote.ExecuteNonQuery();


        }

        public static SqlDataReader ViewUserProjects(int userID)// Updated for JMU Care DB 
        {
            string ViewEmplyProjString = @"
             SELECT * 
             FROM PROJECT 
             WHERE ProjectID IN (
                 SELECT ProjectID 
                 FROM ProjectUsers 
                 WHERE UserID = @UserID
             )";

            SqlCommand cmdViewEmplyProj = new SqlCommand();
            cmdViewEmplyProj.Connection = Lab1DBConnection;
            cmdViewEmplyProj.Connection.ConnectionString = Lab1DBConnString;
            cmdViewEmplyProj.CommandText = ViewEmplyProjString;

            cmdViewEmplyProj.Parameters.AddWithValue("@userID", userID);

            Lab1DBConnection.Open();

            SqlDataReader tempReader = cmdViewEmplyProj.ExecuteReader();
            return tempReader;


        }

        public static bool HashedParameterLogin(string Email, string Password)//Ready for JMU Care DB
        {
            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = Lab1DBConnection;
            cmdLogin.Connection.ConnectionString = AuthConnString;
            cmdLogin.CommandType = CommandType.StoredProcedure;
            cmdLogin.CommandText = "sp_Lab3Login";
            cmdLogin.Parameters.AddWithValue("@Email", Email);

            cmdLogin.Connection.Open();


            SqlDataReader hashReader = cmdLogin.ExecuteReader();
            if (hashReader.Read())
            {
                string correctHash = hashReader["Password"].ToString();

                if (PasswordHash.ValidatePassword(Password, correctHash))
                {
                    return true;
                }
            }

            return false;
        }

        public static void CreateHashedUser(string Email, string Password, int UserID)//Ready for JMU Care DB
        {
            //The int UserID is the userID generated when the user is added to the Lab1DB. it is retrieved in the AddUser() method
            //This will link the login information with a user in the system.
            string loginQuery =
                "INSERT INTO HashedCredentials (Email,Password,UserID) values (@Email, @Password, @UserID)";

            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = Lab1DBConnection;
            cmdLogin.Connection.ConnectionString = AuthConnString;

            cmdLogin.CommandText = loginQuery;
            cmdLogin.Parameters.AddWithValue("@Email", Email);
            cmdLogin.Parameters.AddWithValue("@Password", PasswordHash.HashPassword(Password));
            cmdLogin.Parameters.AddWithValue("@UserID", UserID);

            cmdLogin.Connection.Open();


            cmdLogin.ExecuteNonQuery();

        }

        public static int GetUserID(string Email)//Ready for JMU Care DB
        {
            //This method is called after a successful login to add the userID to the session state to be used for subsequent queries. 
            string getIDQuery = "SELECT UserID FROM HashedCredentials WHERE email = @Email";

            SqlCommand cmdGetID = new SqlCommand();
            cmdGetID.Connection = Lab1DBConnection;
            cmdGetID.Connection.ConnectionString = AuthConnString;

            cmdGetID.CommandText = getIDQuery;
            cmdGetID.Parameters.AddWithValue("@Email", Email);

            cmdGetID.Connection.Open();

            int UserID = (int)cmdGetID.ExecuteScalar();

            return UserID;
        }

        public static SqlDataReader GetUserMessages(string UserID)
        {
            string getMessagesQuery = @"
                SELECT 
                    M.messageID,
                    S.firstName + ' ' + S.lastName AS sender,
                    R.firstName + ' ' + R.lastName AS recipient,
                    M.title,
                    M.content,
                    M.readStatus,
                    M.timestamp
                FROM Message M
                JOIN Users S ON M.senderID = S.userID
                JOIN Users R ON M.recipientID = R.userID
                WHERE M.recipientID = @UserID OR M.senderID = @UserID"; ;

            SqlCommand cmdGetMessages = new SqlCommand();
            cmdGetMessages.Connection = Lab1DBConnection;
            cmdGetMessages.Connection.ConnectionString = Lab1DBConnString;
            cmdGetMessages.CommandText = getMessagesQuery;
            cmdGetMessages.Parameters.AddWithValue("@UserID", UserID);
            cmdGetMessages.Connection.Open();

            SqlDataReader tempreader = cmdGetMessages.ExecuteReader();
            return tempreader;

        }

        public static int NewMessage(int senderID, int RecipientID, string Title, string Content)
        {
            string newMessageQuery = @"
                INSERT INTO Message (senderID, recipientID, title, content, readStatus, timestamp)
                VALUES (@SenderID, @RecipientID, @Title, @Content, 0, @Timestamp)";

            SqlCommand cmdNewMessage = new SqlCommand();
            cmdNewMessage.Connection = Lab1DBConnection;
            cmdNewMessage.Connection.ConnectionString = Lab1DBConnString;
            cmdNewMessage.CommandText = newMessageQuery;

            cmdNewMessage.Parameters.AddWithValue("@SenderID", senderID);
            cmdNewMessage.Parameters.AddWithValue("@RecipientID", RecipientID);
            cmdNewMessage.Parameters.AddWithValue("@Title", Title);
            cmdNewMessage.Parameters.AddWithValue("@Content", Content);
            cmdNewMessage.Parameters.AddWithValue("@Timestamp", DateTime.Now);
            cmdNewMessage.Connection.Open();

            int rowsAffected = cmdNewMessage.ExecuteNonQuery(); // Ensures execution

            return rowsAffected;

        }

        public static SqlDataReader GetUsers() //Updated for JMU Care DB
        {
            string getUsersQuery = "SELECT UserID, FirstName + ' ' + LastName AS FullName FROM Users";

            SqlCommand cmdGetUsers = new SqlCommand();
            cmdGetUsers.Connection = Lab1DBConnection;
            cmdGetUsers.Connection.ConnectionString = Lab1DBConnString;
            cmdGetUsers.CommandText = getUsersQuery;
            cmdGetUsers.Connection.Open();

            SqlDataReader tempreader = cmdGetUsers.ExecuteReader();

            return tempreader;
        }

        public static SqlDataReader ViewProject(Project project) //Updated for JMU Care DB
        {
            string ViewProjectQuery = @"
      SELECT p.ProjectName, p.StartDate, p.EndDate, p.ProjectStatus, p.ProgressStatus, p.ProjectLead, p.UserID, p.GrantID
      FROM Project p
      WHERE (@ProjectName IS NULL OR p.ProjectName LIKE '%' + @ProjectName + '%')
      AND (@StartDate IS NULL OR p.StartDate = @StartDate)
      AND (@EndDate IS NULL OR p.EndDate = @EndDate)
      AND (@ProjectStatus IS NULL OR p.ProjectStatus = @ProjectStatus)
      AND (@ProgressStatus IS NULL OR p.ProgressStatus = @ProgressStatus)
      AND (@ProjectLead IS NULL OR p.ProjectLead = @ProjectLead)
      AND (@UserID IS NULL OR p.UserID = @UserID)
      AND (@GrantID IS NULL OR p.GrantID = @GrantID);";

            SqlCommand cmdViewProjects = new SqlCommand();

            cmdViewProjects.Connection = Lab1DBConnection;
            cmdViewProjects.Connection.ConnectionString = Lab1DBConnString;
            cmdViewProjects.CommandText = ViewProjectQuery;

            cmdViewProjects.Parameters.AddWithValue("@ProjectName",
                string.IsNullOrEmpty(project.ProjectName) ? (object)DBNull.Value : project.ProjectName);

            cmdViewProjects.Parameters.AddWithValue("@StartDate",
                project.StartDate == DateTime.MinValue ? (object)DBNull.Value : project.StartDate);

            cmdViewProjects.Parameters.AddWithValue("@EndDate",
                project.EndDate == DateTime.MinValue ? (object)DBNull.Value : project.EndDate);

            cmdViewProjects.Parameters.AddWithValue("@ProjectStatus",
                string.IsNullOrEmpty(project.ProjectStatus) ? (object)DBNull.Value : project.ProjectStatus);

            cmdViewProjects.Parameters.AddWithValue("@ProgressStatus",
                string.IsNullOrEmpty(project.ProgressStatus) ? (object)DBNull.Value : project.ProgressStatus);

            cmdViewProjects.Parameters.AddWithValue("@ProjectLead",
                project.ProjectLead == 0 ? (object)DBNull.Value : project.ProjectLead);

            cmdViewProjects.Parameters.AddWithValue("@UserID",
                project.UserID == 0 ? (object)DBNull.Value : project.UserID);

            cmdViewProjects.Parameters.AddWithValue("@GrantID",
                project.GrantID == 0 ? (object)DBNull.Value : project.GrantID);

            cmdViewProjects.Connection.Open();

            SqlDataReader tempreader = cmdViewProjects.ExecuteReader();


            return tempreader;


        }

        public static void EditGrant(Grant grant) //Updated for JMU Care DB
        {
            string sqlQuery = @"
    UPDATE [Grants] SET
        GrantName = @GrantName,
        FundingAgency = @FundingAgency,
        Deadline = @Deadline,
        ProposalID = @ProposalID,
        FundingAmount = @FundingAmount,
        Type = @Type,
        GrantDescription = @GrantDescription
    WHERE GrantID = @GrantID";

            SqlCommand cmdGrantUpdate = new SqlCommand();
            cmdGrantUpdate.Connection = Lab1DBConnection;
            cmdGrantUpdate.Connection.ConnectionString = Lab1DBConnString;
            cmdGrantUpdate.CommandText = sqlQuery;

            cmdGrantUpdate.Parameters.AddWithValue("@GrantName", grant.GrantName);
            cmdGrantUpdate.Parameters.AddWithValue("@FundingAgency", grant.FundingAgency);
            cmdGrantUpdate.Parameters.AddWithValue("@Deadline", grant.Deadline);
            cmdGrantUpdate.Parameters.AddWithValue("@ProposalID", grant.ProposalID);
            cmdGrantUpdate.Parameters.AddWithValue("@FundingAmount", grant.FundingAmount);
            cmdGrantUpdate.Parameters.AddWithValue("@Type", grant.Type);
            cmdGrantUpdate.Parameters.AddWithValue("@GrantDescription", grant.GrantDescription);
            cmdGrantUpdate.Parameters.AddWithValue("@GrantID", grant.GrantID);

            cmdGrantUpdate.Connection.Open();
            cmdGrantUpdate.ExecuteNonQuery();
        }

        public static SqlDataReader SingleGrantReader(int GrantID) // Ready for JMU Care DB
        {
            SqlCommand cmdGrantRead = new SqlCommand();
            cmdGrantRead.Connection = Lab1DBConnection;
            cmdGrantRead.Connection.ConnectionString = Lab1DBConnString;
            cmdGrantRead.CommandText = "SELECT * FROM Grants WHERE GrantID = " + GrantID;
            cmdGrantRead.Connection.Open();

            SqlDataReader tempReader = cmdGrantRead.ExecuteReader();

            return tempReader;
        }

        public static SqlDataReader PartnerReader() //Updated for JMU Care DB
        {
            SqlCommand cmdPartnerRead = new SqlCommand();
            cmdPartnerRead.Connection = Lab1DBConnection;
            cmdPartnerRead.Connection.ConnectionString = Lab1DBConnString;
            cmdPartnerRead.CommandText = "SELECT * FROM Partnership";
            cmdPartnerRead.Connection.Open();

            SqlDataReader tempReader = cmdPartnerRead.ExecuteReader();

            return tempReader;
        }

        public static int GetUserType(int userID) //Ready for JMU Care DB
        {
            SqlCommand cmdGetType = new SqlCommand();
            cmdGetType.Connection = Lab1DBConnection;
            cmdGetType.Connection.ConnectionString = Lab1DBConnString;
            cmdGetType.CommandText = "SELECT UserTypeID FROM Users WHERE UserID = @UserID";

            cmdGetType.Parameters.AddWithValue("UserId", userID);
            cmdGetType.Connection.Open();

            int userType = (int)cmdGetType.ExecuteScalar();

            return userType;
        }



        public static SqlDataReader ViewAllRepresenatives() //Updated for JMU Care DB
        {
            SqlCommand cmdViewReps = new SqlCommand();
            cmdViewReps.Connection = Lab1DBConnection;
            cmdViewReps.Connection.ConnectionString = Lab1DBConnString;
            cmdViewReps.CommandText = "SELECT UserID, FirstName, LastName FROM Users WHERE UserTypeID = 4";
            cmdViewReps.Connection.Open();

            SqlDataReader tempreader = cmdViewReps.ExecuteReader();

            return tempreader;
        }

        public static SqlDataReader GrantSearch(Grant Grant) //Updated for JMU Care DB
        {
            string GrantSearchQuery = @"
    SELECT *
    FROM Grant
    WHERE (@GrantName IS NULL OR GrantName LIKE '%' + @GrantName + '%')
      AND (@FundingAgency IS NULL OR FundingAgency LIKE '%' + @FundingAgency + '%')
      AND (@Deadline IS NULL OR Deadline = @Deadline)
      AND (@FundingAmount IS NULL OR FundingAmount = @FundingAmount)
      AND (@Type IS NULL OR Type LIKE '%' + @Type + '%')";

            SqlCommand cmdGrantSearch = new SqlCommand();
            cmdGrantSearch.Connection = Lab1DBConnection;
            cmdGrantSearch.Connection.ConnectionString = Lab1DBConnString;
            cmdGrantSearch.CommandText = GrantSearchQuery;

            cmdGrantSearch.Parameters.AddWithValue("@GrantName",
                string.IsNullOrEmpty(Grant.GrantName) ? (object)DBNull.Value : Grant.GrantName);

            cmdGrantSearch.Parameters.AddWithValue("@FundingAgency",
                string.IsNullOrEmpty(Grant.FundingAgency) ? (object)DBNull.Value : Grant.FundingAgency);

            cmdGrantSearch.Parameters.AddWithValue("@Deadline",
                Grant.Deadline == DateTime.MinValue ? (object)DBNull.Value : Grant.Deadline);

            cmdGrantSearch.Parameters.AddWithValue("@FundingAmount",
                Grant.FundingAmount == 0 ? (object)DBNull.Value : Grant.FundingAmount);

            cmdGrantSearch.Parameters.AddWithValue("@Type",
                string.IsNullOrEmpty(Grant.Type) ? (object)DBNull.Value : Grant.Type);

            cmdGrantSearch.Connection.Open();

            SqlDataReader tempreader = cmdGrantSearch.ExecuteReader();

            return tempreader;


        }

        public static SqlDataReader GetUserTasks(int UserID)
        {
            SqlCommand cmdUserTasks = new SqlCommand();
            cmdUserTasks.Connection = Lab1DBConnection;
            cmdUserTasks.Connection.ConnectionString = Lab1DBConnString;
            cmdUserTasks.CommandText = "SELECT * FROM Task WHERE AssignedTo = @UserID";

            cmdUserTasks.Parameters.AddWithValue("@UserID", UserID);
            cmdUserTasks.Connection.Open();

            SqlDataReader tempreader = cmdUserTasks.ExecuteReader();

            return tempreader;
        }

        public static SqlDataReader GetUserGrants(int UserID)
        {
            SqlCommand cmdUserGrants = new SqlCommand();
            cmdUserGrants.Connection = Lab1DBConnection;
            cmdUserGrants.Connection.ConnectionString = Lab1DBConnString;
            cmdUserGrants.CommandText = @"
                SELECT 
                    GrantApplication.GrantApplicationID,
                    GrantApplication.ApplicationStatus,
                    GrantApplication.PrincipleInvestigator,
                    Grants.GrantID,
                    Grants.GrantName,
                    Grants.FundingAgency,
                    Grants.Deadline,
                    Grants.ProposalID,
                    Grants.FundingAmount,
                    Grants.Type,
                    Grants.GrantDescription
                FROM GrantApplication
                JOIN GrantApplicationUsers 
                    ON GrantApplication.GrantApplicationID = GrantApplicationUsers.GrantApplicationID
                JOIN Grants 
                    ON GrantApplication.GrantID = Grants.GrantID
                WHERE GrantApplicationUsers.UserID = @UserID;";

            cmdUserGrants.Parameters.AddWithValue("@UserID", UserID);
            cmdUserGrants.Connection.Open();

            SqlDataReader tempreader = cmdUserGrants.ExecuteReader();

            return tempreader;
        }

        public static SqlDataReader GetUserDailyTasks(int UserID)
        {
            SqlCommand cmdDailyTasks = new SqlCommand();
            cmdDailyTasks.Connection = Lab1DBConnection;
            cmdDailyTasks.Connection.ConnectionString = Lab1DBConnString;
            cmdDailyTasks.CommandText = "SELECT TaskName, Status, Deadline FROM Task WHERE AssignedTo = @UserID AND CAST(Deadline AS DATE) = @Today; ";

            cmdDailyTasks.Parameters.AddWithValue("@UserID", UserID);
            cmdDailyTasks.Parameters.AddWithValue("@Today", DateTime.Today);
            cmdDailyTasks.Connection.Open();

            SqlDataReader tempreader = cmdDailyTasks.ExecuteReader();

            return tempreader;
        }

        public static String GetUserName(int UserID)
        {
            SqlCommand cmdGetName = new SqlCommand();
            cmdGetName.Connection = Lab1DBConnection;
            cmdGetName.Connection.ConnectionString = Lab1DBConnString;
            cmdGetName.CommandText = "SELECT FirstName, LastName FROM [Users] WHERE UserID = @UserID";

            cmdGetName.Parameters.AddWithValue("@UserID", UserID);
            cmdGetName.Connection.Open();

            String FullName = "";
            SqlDataReader tempreader = cmdGetName.ExecuteReader();

            while (tempreader.Read())
            {
                String FirstName = tempreader.GetString(0);
                String LastName = tempreader.GetString(1);
                FullName = FirstName + LastName;
            }
            Lab1DBConnection.Close();
            return FullName;

        }

        public static int AddEvent(Event NewEvent)
        {
            SqlCommand cmdAddEvent = new SqlCommand();
            cmdAddEvent.Connection = Lab1DBConnection;
            cmdAddEvent.Connection.ConnectionString = Lab1DBConnString;
            cmdAddEvent.CommandText = @"
                        INSERT INTO Event (Title, Description, ScheduledDate)
                        VALUES (@Title, @Description, @EventDateTime);
                        SELECT CAST(SCOPE_IDENTITY() AS int);"; //retrieves newly generated eventID for eventUsers table
            cmdAddEvent.Parameters.AddWithValue("@Title", NewEvent.Title);
            cmdAddEvent.Parameters.AddWithValue("@Description", NewEvent.Description);
            cmdAddEvent.Parameters.AddWithValue("@EventDateTime", NewEvent.EventDateTime);

            cmdAddEvent.Connection.Open();

            int newEventId = (int)cmdAddEvent.ExecuteScalar(); // execute scalar ensures ID is returned from the scoped identity

            Lab1DBConnection.Close();
            return newEventId;
        }

        public static void AddEventUsers(int newEventID, int UserID)
        {
            SqlCommand cmdAddEventUsers = new SqlCommand();
            cmdAddEventUsers.Connection = Lab1DBConnection;
            cmdAddEventUsers.Connection.ConnectionString = Lab1DBConnString;
            cmdAddEventUsers.CommandText = "INSERT INTO EventUsers (EventID, UserID) VALUES (@EventID, @UserID)";
            cmdAddEventUsers.Parameters.AddWithValue("@EventID", newEventID);
            cmdAddEventUsers.Parameters.AddWithValue("@UserID", UserID);

            cmdAddEventUsers.Connection.Open();

            cmdAddEventUsers.ExecuteNonQuery();
        }

        public static SqlDataReader GetUserEvents(int UserID)
        {
            SqlCommand cmdGetEvent = new SqlCommand();
            cmdGetEvent.Connection = Lab1DBConnection;
            cmdGetEvent.Connection.ConnectionString = Lab1DBConnString;
            cmdGetEvent.CommandText = @"
                SELECT * FROM Event
                WHERE EventID IN (
                SELECT EventID FROM EventUsers WHERE UserID = @UserID
                )";
            cmdGetEvent.Parameters.AddWithValue("@UserID", UserID);

            cmdGetEvent.Connection.Open();

            SqlDataReader tempreader = cmdGetEvent.ExecuteReader();

            return tempreader;
        }




        // Returns Users and Projects by Name
        public static SqlDataReader SearchPeopleAndProjects(string searchTerm)
        {
            string query = @"
        SELECT UserID AS ID, FirstName + ' ' + LastName AS Name, 'User' AS Type
        FROM Users
        WHERE FirstName LIKE '%' + @search + '%' OR LastName LIKE '%' + @search + '%'
        UNION
        SELECT ProjectID AS ID, ProjectName AS Name, 'Project' AS Type
        FROM Project
        WHERE ProjectName LIKE '%' + @search + '%'";

            SqlCommand cmd = new SqlCommand(query, Lab1DBConnection);
            cmd.Connection.ConnectionString = Lab1DBConnString;
            cmd.Parameters.AddWithValue("@search", searchTerm);
            Lab1DBConnection.Open();
            return cmd.ExecuteReader();
        }

        // Returns Contact Info from Users
        public static SqlDataReader GetUserByID(int userID)
        {
            string query = "SELECT FirstName, LastName, Email, Department FROM Users WHERE UserID = @UserID";
            SqlCommand cmd = new SqlCommand(query, Lab1DBConnection);
            cmd.Connection.ConnectionString = Lab1DBConnString;
            cmd.Parameters.AddWithValue("@UserID", userID);
            Lab1DBConnection.Open();
            return cmd.ExecuteReader();
        }

        // Returns Users from assigned project  
        public static SqlDataReader GetUsersByProjectID(int projectId)
        {
            string query = @"
        SELECT U.UserID, U.FirstName, U.LastName, U.Email
        FROM ProjectUsers PU
        JOIN Users U ON PU.UserID = U.UserID
        WHERE PU.ProjectID = @ProjectID
        ORDER BY U.FirstName, U.LastName";

            SqlCommand cmd = new SqlCommand(query, Lab1DBConnection);
            cmd.Connection.ConnectionString = Lab1DBConnString;
            cmd.Parameters.AddWithValue("@ProjectID", projectId);
            Lab1DBConnection.Open();
            return cmd.ExecuteReader();
        }
    }
    



}