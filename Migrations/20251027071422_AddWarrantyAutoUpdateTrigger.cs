using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopPC.Migrations
{
    /// <inheritdoc />
    public partial class AddWarrantyAutoUpdateTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE TABLE IF NOT EXISTS WarrantyUpdateLogs (
                id VARCHAR(36) PRIMARY KEY,
                updateDate DATETIME NOT NULL,
                expiredCount INT NOT NULL DEFAULT 0,
                description TEXT,
                createdAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                INDEX idx_update_date (updateDate)
            );
        ");
            migrationBuilder.Sql(@"
            CREATE PROCEDURE SP_AutoUpdateExpiredWarranties()
            BEGIN
                DECLARE updated_count INT DEFAULT 0;
                DECLARE error_message TEXT DEFAULT '';
                
                DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
                BEGIN
                    GET DIAGNOSTICS CONDITION 1 error_message = MESSAGE_TEXT;
                    INSERT INTO WarrantyUpdateLogs (id, updateDate, expiredCount, description, createdAt) 
                    VALUES (UUID(), NOW(), 0, CONCAT('ERROR: ', error_message), NOW());
                END;
                
                UPDATE WarrantyRecords 
                SET status = 'EXPIRED', updatedAt = NOW()
                WHERE endDate < CURDATE() AND status NOT IN ('EXPIRED', 'CANCELLED');
                
                SET updated_count = ROW_COUNT();
                
                INSERT INTO WarrantyUpdateLogs (id, updateDate, expiredCount, description, createdAt) 
                VALUES (UUID(), NOW(), updated_count, CONCAT('SUCCESS: Auto-updated ', updated_count, ' expired warranties'), NOW());
            END");

            migrationBuilder.Sql("SET GLOBAL event_scheduler = ON;");

            // Tạo event
            migrationBuilder.Sql(@"
            CREATE EVENT IF NOT EXISTS AutoUpdateWarrantyEvent
            ON SCHEDULE EVERY 1 DAY
            STARTS (TIMESTAMP(CURRENT_DATE) + INTERVAL 1 DAY + INTERVAL 2 HOUR)
            COMMENT 'Auto update expired warranties daily at 2 AM'
            DO 
                CALL SP_AutoUpdateExpiredWarranties()");
      
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP EVENT IF EXISTS AutoUpdateWarrantyEvent;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS SP_AutoUpdateExpiredWarranties;");
            migrationBuilder.Sql("DROP TABLE IF EXISTS WarrantyUpdateLogs;");
        }
    }
}
