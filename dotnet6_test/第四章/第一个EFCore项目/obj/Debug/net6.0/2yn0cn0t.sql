START TRANSACTION;

ALTER TABLE `T_Book` MODIFY COLUMN `name` varchar(50) CHARACTER SET utf8mb4 NOT NULL COMMENT '书名';

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240313035401_up1', '6.0.28');

COMMIT;

