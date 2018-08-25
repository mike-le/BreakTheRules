using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using BTR.DataAccess.Entities;

namespace BTR.DataAccess
{
    
    public class BTRContext : DbContext
    {
        private EmailConfig _emailconfig;
        public BTRContext(DbContextOptions<BTRContext> options, EmailConfig emailConfig) : base(options)
        {
            _emailconfig = emailConfig;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdeaEntity>()
                .HasMany(i => i.Votes)
                .WithOne(v => v.Idea)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CommentEntity>()
                .HasMany(c => c.Votes)
                .WithOne(v => v.Comment)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<ThemeEntity> Themes { get; set; }
        public DbSet<IdeaEntity> Ideas { get; set; }
        public DbSet<CommentEntity> Comments { get; set;}
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Vote> Votes { get; set; }

        //setup table for audit loggging
        public DbSet<Audit> Audits { get; set; }

        // async savechanges
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,  CancellationToken cancellationToken = default(CancellationToken))
        {
            return SaveChangesWithLoggingAsync(acceptAllChangesOnSuccess, "", cancellationToken);
        }
        public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, string ChangeOwner, CancellationToken cancellationToken = default(CancellationToken))
        {
            return SaveChangesWithLoggingAsync(acceptAllChangesOnSuccess, ChangeOwner, cancellationToken);
        }

        // non async save changes
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return SaveChangesWithLogging(acceptAllChangesOnSuccess, "");
        }
        public int SaveChanges(bool acceptAllChangesOnSuccess, string ChangeOwner = "")
        {
            return SaveChangesWithLogging(acceptAllChangesOnSuccess, ChangeOwner);
        }

        //override for audit logging
        public void SendEmail(ThemeEntity Theme, bool AsReminder = false )
        {
            string SubjectText = "";
            string BodyText = "";
            if (AsReminder)
            {
                SubjectText = _emailconfig.ReminderSubjectText;
                BodyText = _emailconfig.ReminderBodyText;
            }
            else
            {
                SubjectText = _emailconfig.SubjectText;
                BodyText = _emailconfig.BodyText;
            }
            Database.ExecuteSqlCommand("EXEC usp_send_btr_email @FromAddress, @ReplyTo, @Recipients, @BodyText, @SubjectText",
                    new SqlParameter("@FromAddress", _emailconfig.FromAddress),
                    new SqlParameter("@ReplyTo", _emailconfig.ReplyTo),
                    new SqlParameter("@Recipients", _emailconfig.Recipients),
                    new SqlParameter("@BodyText", EmailUtil.FormatThemeString(BodyText, Theme)),
                    new SqlParameter("@SubjectText", EmailUtil.FormatThemeString(SubjectText, Theme))
            );
        }

        // private SaveChanges functions
        private async Task<int> SaveChangesWithLoggingAsync(bool acceptAllChangesOnSuccess, string ChangeOwner, CancellationToken cancellationToken = default(CancellationToken))
        {
            var auditEntries = OnBeforeSaveChanges(ChangeOwner);

            // Check to see if any themes are added during this change
            bool ThemesAdded = ChangeTracker.Entries().Any(e => e.State == EntityState.Added && e.Entity is ThemeEntity);

            ThemeEntity tempTheme = null;
            // Store the theme added before saving
            if (ThemesAdded)
            {
                tempTheme = (ThemeEntity)ChangeTracker.Entries().Where(e => e.State == EntityState.Added && e.Entity is ThemeEntity)
                    .SingleOrDefault().Entity;
            }
            // Save changes
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            // Send email if a theme is added
            if (ThemesAdded && tempTheme != null)
            {
                await Database.ExecuteSqlCommandAsync("EXEC usp_send_btr_email @FromAddress, @ReplyTo, @Recipients, @BodyText, @SubjectText",
                    new SqlParameter("@FromAddress", _emailconfig.FromAddress),
                    new SqlParameter("@ReplyTo", _emailconfig.ReplyTo),
                    new SqlParameter("@Recipients", _emailconfig.Recipients),
                    new SqlParameter("@BodyText", EmailUtil.FormatThemeString(_emailconfig.BodyText, tempTheme)),
                    new SqlParameter("@SubjectText", EmailUtil.FormatThemeString(_emailconfig.SubjectText, tempTheme))
                );
            }
            await OnAfterSaveChangesAsync(auditEntries);
            return result;
        }
        private int SaveChangesWithLogging(bool acceptAllChangesOnSuccess, string ChangeOwner, CancellationToken cancellationToken = default(CancellationToken))
        {
            var auditEntries = OnBeforeSaveChanges(ChangeOwner);

            // Check to see if any themes are added during this change
            bool ThemesAdded = ChangeTracker.Entries().Any(e => e.State == EntityState.Added && e.Entity is ThemeEntity);

            ThemeEntity tempTheme = null;
            // Store the theme added before saving
            if (ThemesAdded)
            {
                tempTheme = (ThemeEntity)ChangeTracker.Entries().Where(e => e.State == EntityState.Added && e.Entity is ThemeEntity)
                    .SingleOrDefault().Entity;
            }
            // Save changes
            var result = base.SaveChanges(acceptAllChangesOnSuccess);

            // Send email if a theme is added
            if (ThemesAdded && tempTheme != null)
            {
                Database.ExecuteSqlCommand("EXEC usp_send_btr_email @FromAddress, @ReplyTo, @Recipients, @BodyText, @SubjectText",
                    new SqlParameter("@FromAddress", _emailconfig.FromAddress),
                    new SqlParameter("@ReplyTo", _emailconfig.ReplyTo),
                    new SqlParameter("@Recipients", _emailconfig.Recipients),
                    new SqlParameter("@BodyText", EmailUtil.FormatThemeString(_emailconfig.BodyText, tempTheme)),
                    new SqlParameter("@SubjectText", EmailUtil.FormatThemeString(_emailconfig.SubjectText, tempTheme))
                );
            }
            OnAfterSaveChanges(auditEntries);
            return result;
        }
        
        // private OnBefore/OnAfter
        private List<AuditEntry> OnBeforeSaveChanges(string ChangeOwner)
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry(entry);
                auditEntry.ChangeOwner = ChangeOwner;
                auditEntry.TableName = entry.Metadata.Relational().TableName;
                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        // value will be generated by the database, get the value after saving
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }

            // Save audit entities that have all the modifications
            foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
            {
                Audits.Add(auditEntry.ToAudit());
            }

            // keep a list of entries where the value of some properties are unknown at this step
            return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
        }
        private Task OnAfterSaveChangesAsync(List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return Task.CompletedTask;
    
            foreach (var auditEntry in auditEntries)
            {
                // Get the final value of the temporary properties
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }

                // Save the Audit entry
                Audits.Add(auditEntry.ToAudit());
            }

            return SaveChangesAsync();
        }
        private void OnAfterSaveChanges(List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return;

            foreach (var auditEntry in auditEntries)
            {
                // Get the final value of the temporary properties
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }

                // Save the Audit entry
                Audits.Add(auditEntry.ToAudit());
            }

            SaveChanges();
            return;
        }
    }
    
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public EntityEntry Entry { get; }
        public string TableName { get; set; }
        public string ChangeOwner { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

        public bool HasTemporaryProperties => TemporaryProperties.Any();

        public Audit ToAudit()
        {
            var audit = new Audit();
            audit.TableName = TableName;
            audit.ChangeOwner = ChangeOwner;
            audit.ChangeDt = DateTime.UtcNow;
            audit.KeyValues = JsonConvert.SerializeObject(KeyValues);
            audit.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
            audit.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
            return audit;
        }
    }
}
