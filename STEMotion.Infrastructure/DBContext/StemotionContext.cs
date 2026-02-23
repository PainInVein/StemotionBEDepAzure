using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using STEMotion.Domain.Entities;
using System;
using System.Collections.Generic;

namespace STEMotion.Infrastructure.DBContext;

public partial class StemotionContext : DbContext
{
    public StemotionContext()
    {
    }

    public StemotionContext(DbContextOptions<StemotionContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<Lesson> Lessons { get; set; }

    public DbSet<Payment> Payments { get; set; }

    public DbSet<Subscription> Subscriptions { get; set; }

    public DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

    public DbSet<Student> Students { get; set; }

    public DbSet<Game> Games { get; set; }
    public DbSet<GameResult> GameResults { get; set; }
    public DbSet<StudentProgress> StudentProgress { get; set; }
    public static string GetConnectionString(string connectionStringName)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = config.GetConnectionString(connectionStringName);
        return connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(GetConnectionString("DefaultConnection"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(255);

            entity.HasIndex(e => e.Name).IsUnique();

            entity.Property(e => e.Description)
                .HasColumnName("description");

            entity.Property(e => e.Status)
                .HasColumnName("status");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");
            entity.HasKey(e => e.UserId);

            entity.Property(e => e.UserId)
                .HasColumnName("userId")
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(255);

            entity.HasIndex(e => e.Email).IsUnique();

            entity.Property(e => e.Password)
                .HasColumnName("password")
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Phone)
                .HasColumnName("phone")
                .HasMaxLength(20);

            entity.Property(e => e.FirstName)
                .HasColumnName("firstName")
                .HasMaxLength(255);

            entity.Property(e => e.LastName)
                .HasColumnName("lastName")
                .HasMaxLength(255);

            entity.Property(e => e.RoleId)
                .HasColumnName("roleId")
                .IsRequired();

            entity.Property(e => e.GradeLevel)
                .HasColumnName("gradeLevel");

            entity.Property(e => e.AvatarUrl)
                .HasColumnName("avatarUrl")
                .IsRequired(false);

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasMaxLength(50);

            entity.Property(e => e.CreatedAt)
                .HasColumnName("createdAt")
                .HasDefaultValueSql("GETDATE()");

            // Foreign Key
            entity.HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId)
                .HasConstraintName("FK_User.role_id")
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.ToTable("Grade");
            entity.HasKey(e => e.GradeId);

            entity.Property(e => e.GradeId)
                .HasColumnName("grade_id")
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.GradeLevel)
                .HasColumnName("grade_level")
                .IsRequired();

            entity.HasIndex(e => e.GradeLevel).IsUnique();

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(255);

            entity.Property(e => e.Description)
                .HasColumnName("description");

            entity.Property(e => e.OrderIndex)
     .HasColumnName("order_index")  // ✅ Map column name
     .IsRequired();

            entity.HasIndex(e => e.OrderIndex)  // ✅ Tạo index
                .HasDatabaseName("IX_Grade_OrderIndex");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.ToTable("Subject");
            entity.HasKey(e => e.SubjectId);

            entity.Property(e => e.SubjectId)
                .HasColumnName("subject_id")
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.GradeId)
                .HasColumnName("grade_id")
                .IsRequired();

            entity.Property(e => e.SubjectName)
                .HasColumnName("name")
                .HasMaxLength(255);

            entity.Property(e => e.Description)
                .HasColumnName("description");

            entity.HasOne(s => s.Grade)
                .WithMany()
                .HasForeignKey(s => s.GradeId)
                .HasConstraintName("FK_Subject.grade_id")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.OrderIndex).IsRequired();
        });

        modelBuilder.Entity<Chapter>(entity =>
        {
            entity.ToTable("Chapter");
            entity.HasKey(e => e.ChapterId);

            entity.Property(e => e.ChapterId)
                .HasColumnName("chapter_id")
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.SubjectId)
                .HasColumnName("subject_id")
                .IsRequired();

            entity.Property(e => e.ChapterName)
                .HasColumnName("chapter_name")
                .HasMaxLength(255);

            entity.HasOne(c => c.Subject)
                .WithMany(s => s.Chapters)
                .HasForeignKey(c => c.SubjectId)
                .HasConstraintName("FK_Chapter.subject_id")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.OrderIndex).IsRequired();
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.ToTable("Lesson");
            entity.HasKey(e => e.LessonId);

            entity.Property(e => e.LessonId)
                .HasColumnName("lesson_id")
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.ChapterId)
                .HasColumnName("chapter_id")
                .IsRequired();

            entity.Property(e => e.LessonName)
                .HasColumnName("lessonName")
                .HasMaxLength(255);

            entity.Property(e => e.EstimatedTime)
                .HasColumnName("estimated_time");

            entity.HasOne(l => l.Chapter)
                .WithMany(c => c.Lessons)
                .HasForeignKey(l => l.ChapterId)
                .HasConstraintName("FK_Lesson.chapter_id")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.OrderIndex).IsRequired();

        });
        modelBuilder.Entity<LessonContent>(entity =>
        {
            entity.ToTable("LessonContent");
            entity.HasKey(e => e.LessonContentId);

            entity.Property(e => e.LessonContentId)
                .HasColumnName("lesson_content_id")
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.LessonId)
                .HasColumnName("lesson_id")
                .IsRequired();

            entity.Property(e => e.ContentType)
                .HasColumnName("content_type")
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.TextContent)
                .HasColumnName("text_content")
                .IsRequired(false);

            entity.Property(e => e.MediaUrl)
                .HasColumnName("media_url")
                .HasMaxLength(500)
                .IsRequired(false);

            entity.Property(e => e.FormulaLatex)
                .HasColumnName("formula_latex")
                .IsRequired(false);

            entity.Property(e => e.OrderIndex)
                .HasColumnName("order_index")
                .IsRequired();

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasMaxLength(50)
                .HasDefaultValue("Active");

            entity.HasOne(lc => lc.Lesson)
                .WithMany(l => l.LessonContents)
                .HasForeignKey(lc => lc.LessonId)
                .HasConstraintName("FK_LessonContent.lesson_id")
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payment");

            entity.HasKey(e => e.PaymentId);

            entity.Property(e => e.PaymentId)
                  .HasColumnName("payment_id")
                  .HasDefaultValueSql("NEWID()")
                  .IsRequired();

            entity.Property(e => e.UserId)
                  .HasColumnName("user_id")
                  .IsRequired();

            entity.Property(e => e.Info)
                  .HasColumnName("info")
                  .HasMaxLength(500)
                  .IsRequired();

            entity.Property(e => e.Amount)
                  .HasColumnName("amount")
                  .HasPrecision(18, 2)
                  .IsRequired();

            entity.Property(e => e.PaymentDate)
                  .HasColumnName("payment_date")
                  .HasDefaultValueSql("GETDATE()")
                  .IsRequired();

            entity.Property(e => e.Status)
                  .HasColumnName("status")
                  .HasMaxLength(50)
                  .IsRequired();

            entity.HasOne(p => p.User)
                  .WithMany(u => u.Payments)
                  .HasForeignKey(p => p.UserId)
                  .HasConstraintName("FK_Payment_UserId")
                  .OnDelete(DeleteBehavior.Restrict);

      //      entity.HasOne(p => p.SubscriptionPayments)
      //.WithOne(sp => sp.Payment)
      //.HasForeignKey<SubscriptionPayment>(sp => sp.PaymentId)
      //.OnDelete(DeleteBehavior.Restrict);
        });





        // SubscriptionPayment Entity Configuration
        modelBuilder.Entity<SubscriptionPayment>(entity =>
        {
            entity.ToTable("SubscriptionPayment");

            entity.HasKey(e => e.SubscriptionPaymentId);

            entity.Property(e => e.SubscriptionPaymentId)
                .HasColumnName("subscription_payment_id")
                .HasDefaultValueSql("NEWID()")
                .IsRequired();

            entity.Property(e => e.PaymentId)
                .HasColumnName("payment_id")
                .IsRequired();

            entity.Property(e => e.SubscriptionId)
                .HasColumnName("subscription_id")
                .IsRequired();

            entity.Property(e => e.Code)
                .HasColumnName("code")
                .HasMaxLength(10)
                .IsRequired(false);


            entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasMaxLength(255)
                .IsRequired(false);

            entity.Property(e => e.Success)
                .HasColumnName("is_success")
                .IsRequired(false);

            entity.Property(e => e.AccountNumber)
                .HasColumnName("account_number")
                .HasMaxLength(50)
                .IsRequired(false);

            entity.Property(e => e.Amount)
                .HasColumnName("amount")
                .HasPrecision(18, 2)
                .IsRequired(false);

            entity.Property(e => e.Currency)
                .HasColumnName("currency")
                .HasMaxLength(10)
                .IsRequired(false);

            entity.Property(e => e.OrderCode)
                .HasColumnName("order_code")
                .IsRequired(false);

            entity.Property(e => e.Reference)
                .HasColumnName("reference")
                .HasMaxLength(100)
                .IsRequired(false);

            entity.Property(e => e.PaymentLinkId)
                .HasColumnName("payment_link_id")
                .HasMaxLength(100)
                .IsRequired(false);

            entity.Property(e => e.TransactionDateTime)
                .HasColumnName("transaction_datetime")
                .IsRequired(false);

            entity.Property(e => e.CounterAccountBankId)
                .HasColumnName("counter_account_bank_id")
                .HasMaxLength(50)
                .IsRequired(false);

            entity.Property(e => e.CounterAccountName)
                .HasColumnName("counter_account_name")
                .HasMaxLength(255)
                .IsRequired(false);

            entity.Property(e => e.CounterAccountNumber)
                .HasColumnName("counter_account_number")
                .HasMaxLength(50)
                .IsRequired(false);

            //// Foreign keys
            //entity.HasOne(sp => sp.Payment)
            //    .WithMany()
            //    .HasForeignKey(sp => sp.PaymentId)
            //    .HasConstraintName("FK_SubscriptionPayment_Payment")
            //    .OnDelete(DeleteBehavior.Restrict);

            //entity.HasOne(sp => sp.Subscription)
            //    .WithMany()
            //    .HasForeignKey(sp => sp.SubscriptionId)
            //    .HasConstraintName("FK_SubscriptionPayment_Subscription")
            //    .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(sp => sp.Payment)
          .WithOne(p => p.SubscriptionPayment)
          .HasForeignKey<SubscriptionPayment>(sp => sp.PaymentId)
          .OnDelete(DeleteBehavior.Restrict);

        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.ToTable("Game");
            entity.HasKey(e => e.GameId);

            entity.Property(e => e.GameId)
                .HasColumnName("game_id")
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.GameCode)
                .HasColumnName("game_code")
                .IsRequired()
                .HasMaxLength(100);

            // Unique constraint cho GameCode
            entity.HasIndex(e => e.GameCode)
                .IsUnique()
                .HasDatabaseName("IX_Game_GameCode");

            entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasMaxLength(1000);

            entity.Property(e => e.LessonId)
                .HasColumnName("lesson_id")
                .IsRequired();

            entity.Property(e => e.ConfigData)
                .HasColumnName("config_data")
                .IsRequired()
                .HasColumnType("nvarchar(max)"); // JSON data

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .IsRequired()
                .HasDefaultValue(true);

            entity.Property(e => e.ThumbnailUrl)
                .HasColumnName("thumbnail_url")
                .HasMaxLength(500);

            // Foreign Key
            entity.HasOne(g => g.Lesson)
                .WithMany()
                .HasForeignKey(g => g.LessonId)
                .HasConstraintName("FK_Game.lesson_id")
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ⭐ CONFIGURATION CHO GAMERESULT
        modelBuilder.Entity<GameResult>(entity =>
        {
            entity.ToTable("GameResult");
            entity.HasKey(e => e.GameResultId);

            entity.Property(e => e.GameResultId)
                .HasColumnName("game_result_id")
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.StudentId)
                .HasColumnName("student_id")
                .IsRequired();

            entity.Property(e => e.GameId)
                .HasColumnName("game_id")
                .IsRequired();

            entity.Property(e => e.Score)
                .HasColumnName("score")
                .IsRequired()
                .HasColumnType("decimal(5,2)"); // Ví dụ: 95.50

            entity.Property(e => e.CorrectAnswers)
                .HasColumnName("correct_answers")
                .IsRequired();

            entity.Property(e => e.TotalQuestions)
                .HasColumnName("total_questions")
                .IsRequired();

            entity.Property(e => e.PlayDuration)
                .HasColumnName("play_duration")
                .IsRequired(); // Seconds

            entity.Property(e => e.PlayedAt)
                .HasColumnName("played_at")
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            // Index để query nhanh hơn
            entity.HasIndex(e => new { e.StudentId, e.GameId })
                .HasDatabaseName("IX_GameResult_StudentId_GameId");

            entity.HasIndex(e => e.PlayedAt)
                .HasDatabaseName("IX_GameResult_PlayedAt");

            // Foreign Keys
            entity.HasOne(gr => gr.Student)
                .WithMany(s => s.GameResults)
                .HasForeignKey(gr => gr.StudentId)
                .HasConstraintName("FK_GameResult.student_id")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(gr => gr.Game)
                .WithMany(g => g.GameResults)
                .HasForeignKey(gr => gr.GameId)
                .HasConstraintName("FK_GameResult.game_id")
                .OnDelete(DeleteBehavior.Cascade); // Xóa game thì xóa luôn kết quả
        });


        //Subscription Entity Configuration
        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.ToTable("Subscription");

            entity.HasKey(e => e.SubscriptionId);

            entity.Property(e => e.SubscriptionId)
                .HasColumnName("subscription_id")
                .HasDefaultValueSql("NEWID()")
                .IsRequired();

            entity.Property(e => e.SubscriptionName)
                .HasColumnName("subscription_name")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.Description)
                .HasColumnName("description");

            entity.Property(e => e.SubscriptionPrice)
                .HasColumnName("subscription_price")
                .HasPrecision(18, 2)
                .IsRequired();

            entity.Property(e => e.BillingPeriod)
                .HasColumnName("billing_period")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("GETDATE()");

            entity.HasMany(s => s.SubscriptionPayments)
      .WithOne(sp => sp.Subscription)
      .HasForeignKey(sp => sp.SubscriptionId)
      .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<StudentProgress>(entity =>
        {
            entity.ToTable("StudentProgress");

            entity.HasKey(sp => sp.StudentProgressId);

            entity.Property(sp => sp.StudentProgressId)
                .HasColumnName("student_progress_id")
                .HasDefaultValueSql("NEWID()");

            entity.Property(sp => sp.StudentId)
                .HasColumnName("student_id")
                .IsRequired();

            entity.Property(sp => sp.LessonId)
                .HasColumnName("lesson_id")
                .IsRequired();

            entity.Property(sp => sp.IsCompleted)
                .HasColumnName("is_completed")
                .HasDefaultValue(false);

            entity.Property(sp => sp.CompletionPercentage)
                .HasColumnName("completion_percentage");

            entity.Property(sp => sp.StartedAt)
                .HasColumnName("started_at");

            entity.Property(sp => sp.CompletedAt)
                .HasColumnName("completed_at");

            entity.Property(sp => sp.LastAccessedAt)
                .HasColumnName("last_accessed_at");

            entity.Property(sp => sp.Status)
                .HasColumnName("status")
                .HasMaxLength(50)
                .HasDefaultValue("NotStarted");

            entity.HasOne(sp => sp.Student)
                .WithMany(s => s.StudentProgress)
                .HasForeignKey(sp => sp.StudentId)
                .HasConstraintName("FK_StudentProgress_student_id")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(sp => sp.Lesson)
                .WithMany(l => l.StudentProgress)
                .HasForeignKey(sp => sp.LessonId)
                .HasConstraintName("FK_StudentProgress_lesson_id")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(sp => new { sp.StudentId, sp.LessonId })
                .IsUnique()
                .HasDatabaseName("IX_StudentProgress_Student_Lesson");
        });


        //student table
        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Student");

            entity.HasKey(e => e.StudentId);

            entity.Property(e => e.StudentId)
                .HasColumnName("student_id")
                .HasDefaultValueSql("NEWID()")
                .IsRequired();

            entity.Property(e => e.ParentId)
                .HasColumnName("parent_id")
                .IsRequired();

            entity.Property(e => e.Username)
                .HasColumnName("username")
                .HasMaxLength(100);

            entity.HasIndex(e => e.Username)
                .IsUnique()
                .HasDatabaseName("IX_Student_Username");  

            entity.Property(e => e.Password)
                .HasColumnName("password")
                .HasMaxLength(255);

            entity.Property(e => e.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(255);

            entity.Property(e => e.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(255);

            entity.Property(e => e.AvatarUrl)
                .HasColumnName("avatar_url")
                .HasMaxLength(500);

            entity.Property(e => e.GradeLevel)
                .HasColumnName("grade_level");

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasMaxLength(50)
                .HasDefaultValue("Active");

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("GETDATE()");

            // Relationship to Parent (User)
            entity.HasOne(s => s.Parent)
                  .WithMany(u => u.Students)
                  .HasForeignKey(s => s.ParentId)
                  .HasConstraintName("FK_Student_parent_id") 
                  .OnDelete(DeleteBehavior.Restrict);
        });

    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
