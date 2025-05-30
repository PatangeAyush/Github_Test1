USE [DapperAPI]
GO
/****** Object:  Table [dbo].[tbl_Category_Master]    Script Date: 5/6/2025 8:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Category_Master](
	[Category_ID] [int] IDENTITY(1,1) NOT NULL,
	[Category_Name] [varchar](100) NULL,
	[URLPath] [varchar](150) NULL,
	[Icon] [varchar](150) NULL,
	[Banner] [varchar](150) NULL,
	[IsPublished] [bit] NULL,
	[IsIncludeMenu] [bit] NULL,
	[Created_Date] [datetime] NULL,
	[Updated_Date] [datetime] NULL,
	[IsDelete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Category_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Master_SubCategory]    Script Date: 5/6/2025 8:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Master_SubCategory](
	[Master_SubCategory_ID] [int] IDENTITY(1,1) NOT NULL,
	[Master_SubCategory_Name] [varchar](100) NULL,
	[Category_ID] [int] NULL,
	[URLPath] [varchar](150) NULL,
	[Icon] [varchar](150) NULL,
	[Banner] [varchar](150) NULL,
	[IsPublished] [bit] NULL,
	[IsIncludeMenu] [bit] NULL,
	[Created_Date] [datetime] NULL,
	[Updated_Date] [datetime] NULL,
	[IsDelete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Master_SubCategory_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Product_Image]    Script Date: 5/6/2025 8:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Product_Image](
	[Product_ID] [int] NULL,
	[Image_Path] [varchar](300) NULL,
	[IsPrimary] [bit] NULL,
	[Sort_Order] [int] NULL,
	[Created_Date] [datetime] NULL,
	[IsDelete] [bit] NULL,
	[Image_ID] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Image_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Product_Master]    Script Date: 5/6/2025 8:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Product_Master](
	[Product_Name] [varchar](150) NULL,
	[Product_Icon] [varchar](150) NULL,
	[Product_Code] [varchar](50) NULL,
	[MasterCategory_ID] [int] NULL,
	[MasterSubCategory_ID] [int] NULL,
	[SubCategory_ID] [int] NULL,
	[Short_Description] [varchar](500) NULL,
	[Long_Description] [text] NULL,
	[Price] [decimal](10, 2) NULL,
	[MRP] [decimal](10, 2) NULL,
	[Discount_Percent] [int] NULL,
	[Stock_Quantity] [int] NULL,
	[IsFeatured] [bit] NULL,
	[IsAvailable] [bit] NULL,
	[Created_Date] [datetime] NULL,
	[Updated_Date] [datetime] NULL,
	[IsDelete] [bit] NULL,
	[Product_ID] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Product_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_SubCategory]    Script Date: 5/6/2025 8:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_SubCategory](
	[SubCategory_ID] [int] IDENTITY(1,1) NOT NULL,
	[SubCategory_Name] [varchar](100) NULL,
	[Master_SubCategory_ID] [int] NULL,
	[URLPath] [varchar](150) NULL,
	[Icon] [varchar](150) NULL,
	[Banner] [varchar](150) NULL,
	[IsPublished] [bit] NULL,
	[IsIncludeMenu] [bit] NULL,
	[Created_Date] [datetime] NULL,
	[Updated_Date] [datetime] NULL,
	[IsDelete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[SubCategory_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[SP_CategoryMaster]    Script Date: 5/6/2025 8:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_CategoryMaster]
    @action VARCHAR(100),
    @Category_ID INT = NULL,
    @Category_Name VARCHAR(100) = NULL,
    @URLPath VARCHAR(100) = NULL,
    @Icon VARCHAR(100) = NULL,
    @Banner VARCHAR(100) = NULL,
    @IsPublished BIT = 0,
    @IsIncludeMenu BIT = 0
AS
BEGIN
    -- Get All Categories
    IF @action = 'GetCategories'
    BEGIN
        SELECT 
            Category_ID, 
            Category_Name, 
            URLPath, 
            Icon, 
            Banner, 
            IsPublished, 
            IsIncludeMenu, 
            Created_Date, 
            Updated_Date 
        FROM tbl_Category_Master
        WHERE IsDelete = 0;
    END

    -- Add New Category
    ELSE IF @action = 'AddCategory'
    BEGIN
        INSERT INTO tbl_Category_Master 
        (
            Category_Name, 
            URLPath, 
            Icon, 
            Banner, 
            IsPublished, 
            IsIncludeMenu,
            Created_Date,
			IsDelete
        )
        VALUES 
        (
            @Category_Name, 
            @URLPath, 
            @Icon, 
            @Banner, 
            @IsPublished, 
            @IsIncludeMenu,
            GETDATE(),
			0
        );
    END

    -- Update Existing Category
    ELSE IF @action = 'UpdateCategory'
    BEGIN
        UPDATE tbl_Category_Master
        SET 
            Category_Name = @Category_Name,
            URLPath = @URLPath,
            Icon = COALESCE(@Icon, Icon),
            Banner = COALESCE(@Banner, Banner),
            IsPublished = @IsPublished,
            IsIncludeMenu = @IsIncludeMenu,
            Updated_Date = GETDATE()
        WHERE Category_ID = @Category_ID;
    END

    -- Delete Category (Soft Delete)
    ELSE IF @action = 'DeleteCategory'
    BEGIN
        UPDATE tbl_Category_Master
        SET IsDelete = 1, Updated_Date = GETDATE()
        WHERE Category_ID = @Category_ID;
    END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ManageProduct]    Script Date: 5/6/2025 8:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_ManageProduct]

    @Action VARCHAR(50),

    -- Product Parameters (tbl_Product_Master)
    @ProductID INT = NULL,
    @ProductName VARCHAR(100) = NULL,
    @ProductIcon VARCHAR(MAX) = NULL, 
    @ProductCode VARCHAR(50) = NULL,
    @MasterCategoryID INT = NULL,
    @MasterSubCategoryID INT = NULL,
    @SubCategoryID INT = NULL,
    @ShortDescription VARCHAR(500) = NULL,
    @LongDescription VARCHAR(MAX) = NULL,
    @Price DECIMAL(18, 2) = NULL,
    @MRP DECIMAL(18, 2) = NULL,
    @DiscountPercent DECIMAL(5, 2) = NULL,
    @StockQuantity INT = NULL,
    @IsFeatured BIT = 0,
    @IsAvailable BIT = 1, 

    -- Image Parameters (tbl_Product_Image)
    @ImageID INT = NULL,
    @ImagePath VARCHAR(300) = NULL,
    @IsPrimary BIT = 0,
    @SortOrder INT = 0 

AS
BEGIN
    SET NOCOUNT ON;

    -- ==================================================================
    -- ACTION: GetProductByID

    IF @Action = 'GetProductByID'
    BEGIN
        IF @ProductID IS NULL
        BEGIN
            SELECT 'ProductID is required for GetProductByID action.' AS Result, 0 AS Success;
            RETURN;
        END

        SELECT
            p.Product_ID, p.Product_Name, p.Product_Icon, p.Product_Code,
            p.MasterCategory_ID, mc.Category_Name, 
            p.MasterSubCategory_ID, msc.Master_SubCategory_Name, 
            p.SubCategory_ID, sc.SubCategory_Name,
            p.Short_Description, p.Long_Description, p.Price, p.MRP,
            p.Discount_Percent, p.Stock_Quantity, p.IsFeatured, p.IsAvailable,
            p.Created_Date, p.Updated_Date
        FROM dbo.tbl_Product_Master p
        LEFT JOIN dbo.tbl_Category_master mc ON p.MasterCategory_ID = mc.Category_ID -- Adjust join table/columns
        LEFT JOIN dbo.tbl_Master_SubCategory msc ON p.MasterSubCategory_ID = msc.Master_SubCategory_ID -- Adjust join table/columns
        LEFT JOIN dbo.tbl_SubCategory sc ON p.SubCategory_ID = sc.SubCategory_ID -- Adjust join table/columns
        WHERE p.Product_ID = @ProductID AND p.IsDelete = 0;

        -- Product Images
        SELECT
            Image_ID, Product_ID, Image_Path, IsPrimary, Sort_Order, Created_Date
        FROM dbo.tbl_Product_Image
        WHERE Product_ID = @ProductID AND IsDelete = 0
        ORDER BY Sort_Order, IsPrimary DESC, Image_ID; -- Show primary first

        SELECT 'Success' AS Result, 1 AS Success;
    END

    -- ==================================================================
    -- ACTION: GetAllProducts

    ELSE IF @Action = 'GetAllProducts'
    BEGIN
        SELECT
            p.Product_ID, p.Product_Name, p.Product_Icon, p.Product_Code,
            p.MasterCategory_ID, mc.Category_Name, -- Assuming tbl_MasterCategory exists
            p.MasterSubCategory_ID, msc.Master_SubCategory_Name, -- Assuming tbl_Master_SubCategory exists
            p.SubCategory_ID, sc.SubCategory_Name, -- Assuming tbl_SubCategory exists
            p.Short_Description, p.Long_Description, p.Price, p.MRP,
            p.Discount_Percent, p.Stock_Quantity, p.IsFeatured, p.IsAvailable,
            p.Created_Date, p.Updated_Date
        FROM dbo.tbl_Product_Master p
        LEFT JOIN dbo.tbl_Category_Master mc ON p.MasterCategory_ID = mc.Category_ID -- Adjust join table/columns
        LEFT JOIN dbo.tbl_Master_SubCategory msc ON p.MasterSubCategory_ID = msc.Master_SubCategory_ID -- Adjust join table/columns
        LEFT JOIN dbo.tbl_SubCategory sc ON p.SubCategory_ID = sc.SubCategory_ID -- Adjust join table/columns
        WHERE p.IsDelete = 0;

        SELECT
            img.Image_ID, img.Product_ID, img.Image_Path, img.IsPrimary, img.Sort_Order, img.Created_Date
        FROM tbl_Product_Image img
        INNER JOIN dbo.tbl_Product_Master p ON img.Product_ID = p.Product_ID
        WHERE img.IsDelete = 0 AND p.IsDelete = 0;

        SELECT 'Success' AS Result, 1 AS Success;
    END

    -- ==================================================================
    -- ACTION: AddProduct

    ELSE IF @Action = 'AddProduct'
    BEGIN
        -- Basic Validation
        IF @ProductName IS NULL OR @Price IS NULL OR @StockQuantity IS NULL -- Add other mandatory fields as needed
        BEGIN
             SELECT 'ProductName, Price, MasterCategoryID, StockQuantity are required for AddProduct action.' AS Result, 0 AS Success;
             RETURN;
        END

        BEGIN TRANSACTION;
        BEGIN TRY
            INSERT INTO dbo.tbl_Product_Master (
                Product_Name, Product_Icon, Product_Code,
                MasterCategory_ID, MasterSubCategory_ID, SubCategory_ID,
                Short_Description, Long_Description, Price, MRP, Discount_Percent,
                Stock_Quantity, IsFeatured, IsAvailable,
                Created_Date, Updated_Date, IsDelete
            ) VALUES (
                @ProductName, @ProductIcon, @ProductCode,
                @MasterCategoryID, @MasterSubCategoryID, @SubCategoryID,
                @ShortDescription, @LongDescription, @Price, @MRP, @DiscountPercent,
                @StockQuantity, @IsFeatured, @IsAvailable,
                GETDATE(), GETDATE(), 0 -- Created_Date, Updated_Date, IsDelete
            );

            DECLARE @NewProductID INT = SCOPE_IDENTITY();
            COMMIT TRANSACTION;

            SELECT 'Product added successfully.' AS Result, 1 AS Success, @NewProductID AS ProductID;

        END TRY
        BEGIN CATCH
            IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
            PRINT 'Error Occurred While Adding Product: ' + ERROR_MESSAGE();
            SELECT 'Error occurred while adding product.' AS Result, 0 AS Success, ERROR_MESSAGE() AS ErrorDetails;
        END CATCH;
    END

    -- ==================================================================
    -- ACTION: UpdateProduct

    ELSE IF @Action = 'UpdateProduct'
    BEGIN
        -- Basic Validation
        IF @ProductID IS NULL
        BEGIN
            SELECT 'ProductID is required for UpdateProduct action.' AS Result, 0 AS Success;
            RETURN;
        END

        IF NOT EXISTS (SELECT 1 FROM dbo.tbl_Product_Master WHERE Product_ID = @ProductID AND IsDelete = 0)
        BEGIN
            SELECT 'Product not found or has been deleted.' AS Result, 0 AS Success;
            RETURN;
        END

        BEGIN TRANSACTION;
        BEGIN TRY
            UPDATE dbo.tbl_Product_Master
            SET
                Product_Name = ISNULL(@ProductName, Product_Name), -- Use ISNULL or COALESCE if you want partial updates
                Product_Icon = ISNULL(@ProductIcon, Product_Icon),
                Product_Code = ISNULL(@ProductCode, Product_Code),
                MasterCategory_ID = ISNULL(@MasterCategoryID, MasterCategory_ID),
                MasterSubCategory_ID = ISNULL(@MasterSubCategoryID, MasterSubCategory_ID),
                SubCategory_ID = ISNULL(@SubCategoryID, SubCategory_ID),
                Short_Description = ISNULL(@ShortDescription, Short_Description),
                Long_Description = ISNULL(@LongDescription, Long_Description),
                Price = ISNULL(@Price, Price),
                MRP = ISNULL(@MRP, MRP),
                Discount_Percent = ISNULL(@DiscountPercent, Discount_Percent),
                Stock_Quantity = ISNULL(@StockQuantity, Stock_Quantity),
                IsFeatured = ISNULL(@IsFeatured, IsFeatured),
                IsAvailable = ISNULL(@IsAvailable, IsAvailable),
                Updated_Date = GETDATE()
            WHERE Product_ID = @ProductID AND IsDelete = 0;

            COMMIT TRANSACTION;
            SELECT 'Product updated successfully.' AS Result, 1 AS Success, @ProductID AS ProductID;

        END TRY
        BEGIN CATCH
            IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
            PRINT 'Error Occurred While Updating Product: ' + ERROR_MESSAGE();
            SELECT 'Error occurred while updating product.' AS Result, 0 AS Success, ERROR_MESSAGE() AS ErrorDetails;
        END CATCH;
    END

    -- ==================================================================
    -- ACTION: DeleteProduct (Soft Delete)

    ELSE IF @Action = 'DeleteProduct'
    BEGIN
        IF @ProductID IS NULL
        BEGIN
            SELECT 'ProductID is required for DeleteProduct action.' AS Result, 0 AS Success;
            RETURN;
        END

        IF NOT EXISTS (SELECT 1 FROM dbo.tbl_Product_Master WHERE Product_ID = @ProductID AND IsDelete = 0)
        BEGIN
            SELECT 'Product not found or already deleted.' AS Result, 0 AS Success; -- Changed message slightly
            RETURN;
        END

        BEGIN TRANSACTION;
        BEGIN TRY
            -- Soft delete associated images (Requires IsDelete column in tbl_Product_Image)
            UPDATE dbo.tbl_Product_Image
            SET IsDelete = 1
            WHERE Product_ID = @ProductID;

            -- Soft delete the product
            UPDATE dbo.tbl_Product_Master
            SET IsDelete = 1, Updated_Date = GETDATE()
            WHERE Product_ID = @ProductID;

            COMMIT TRANSACTION;
            SELECT 'Product and associated images marked as deleted.' AS Result, 1 AS Success, @ProductID AS ProductID;

        END TRY
        BEGIN CATCH
            IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
            PRINT 'Error Occurred While Deleting Product: ' + ERROR_MESSAGE();
            SELECT 'Error occurred while deleting product.' AS Result, 0 AS Success, ERROR_MESSAGE() AS ErrorDetails;
        END CATCH;
    END

    -- ==================================================================
    -- ACTION: AddProductImage

    ELSE IF @Action = 'AddProductImage'
    BEGIN
        IF @ProductID IS NULL OR @ImagePath IS NULL
        BEGIN
            SELECT 'ProductID and ImagePath are required for AddProductImage action.' AS Result, 0 AS Success;
            RETURN;
        END

        IF NOT EXISTS (SELECT 1 FROM dbo.tbl_Product_Master WHERE Product_ID = @ProductID AND IsDelete = 0)
        BEGIN
            SELECT 'Product not found or has been deleted. Cannot add image.' AS Result, 0 AS Success;
            RETURN;
        END

        BEGIN TRANSACTION;
        BEGIN TRY
            -- If this image is set as primary, unset any other primary images for this product
            IF @IsPrimary = 1
            BEGIN
                UPDATE dbo.tbl_Product_Image
                SET IsPrimary = 0
                WHERE Product_ID = @ProductID AND IsPrimary = 1 AND IsDelete = 0;
            END

         
            INSERT INTO dbo.tbl_Product_Image (
                Product_ID, Image_Path, IsPrimary, Sort_Order, Created_Date, IsDelete
            ) VALUES (
                @ProductID, @ImagePath, @IsPrimary, @SortOrder, GETDATE(), 0
            );

            DECLARE @NewImageID INT = SCOPE_IDENTITY();
            COMMIT TRANSACTION;
            SELECT 'Product image added successfully.' AS Result, 1 AS Success, @NewImageID AS ImageID;

        END TRY
        BEGIN CATCH
            IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
            PRINT 'Error Occurred While Adding Product Image: ' + ERROR_MESSAGE();
            SELECT 'Error occurred while adding product image.' AS Result, 0 AS Success, ERROR_MESSAGE() AS ErrorDetails;
        END CATCH;
    END

END
GO
/****** Object:  StoredProcedure [dbo].[SP_MasterSubCategory]    Script Date: 5/6/2025 8:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_MasterSubCategory]
    @action VARCHAR(100),
    @Master_SubCategory_ID INT = NULL,
    @Master_SubCategory_Name VARCHAR(100) = NULL,
    @Category_ID INT = NULL,
    @URLPath VARCHAR(150) = NULL,
    @Icon VARCHAR(150) = NULL,
    @Banner VARCHAR(150) = NULL,
    @IsPublished BIT = 0,
    @IsIncludeMenu BIT = 0
AS
BEGIN
    IF @action = 'GetMasterSubCategories'
    BEGIN
        SELECT 
            m.Master_SubCategory_ID,
            m.Master_SubCategory_Name,
            m.Category_ID,
            c.Category_Name,
            m.URLPath,
            m.Icon,
            m.Banner,
            m.IsPublished,
            m.IsIncludeMenu,
            m.Created_Date,
            m.Updated_Date
        FROM tbl_Master_SubCategory m
        INNER JOIN tbl_Category_Master c ON m.Category_ID = c.Category_ID
        WHERE m.IsDelete = 0 AND c.IsDelete = 0;
    END

    ELSE IF @action = 'AddMasterSubCategory'
    BEGIN
        INSERT INTO tbl_Master_SubCategory 
        (
            Master_SubCategory_Name, 
            Category_ID, 
            URLPath, 
            Icon, 
            Banner, 
            IsPublished, 
            IsIncludeMenu,
            Created_Date,
            IsDelete
        )
        VALUES 
        (
            @Master_SubCategory_Name, 
            @Category_ID, 
            @URLPath, 
            @Icon, 
            @Banner, 
            @IsPublished, 
            @IsIncludeMenu,
            GETDATE(),
            0
        );
    END

    ELSE IF @action = 'UpdateMasterSubCategory'
    BEGIN
        UPDATE tbl_Master_SubCategory
        SET 
            Master_SubCategory_Name = @Master_SubCategory_Name,
            Category_ID = @Category_ID,
            URLPath = @URLPath,
            Icon = COALESCE(@Icon, Icon),
            Banner = COALESCE(@Banner, Banner),
            IsPublished = @IsPublished,
            IsIncludeMenu = @IsIncludeMenu,
            Updated_Date = GETDATE()
        WHERE Master_SubCategory_ID = @Master_SubCategory_ID;
    END

    ELSE IF @action = 'DeleteMasterSubCategory'
    BEGIN
        UPDATE tbl_Master_SubCategory
        SET IsDelete = 1, Updated_Date = GETDATE()
        WHERE Master_SubCategory_ID = @Master_SubCategory_ID;
    END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_SubCategory]    Script Date: 5/6/2025 8:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_SubCategory]
    @action VARCHAR(100),
    @SubCategory_ID INT = NULL,
    @SubCategory_Name VARCHAR(100) = NULL,
    @Master_SubCategory_ID INT = NULL,
    @URLPath VARCHAR(150) = NULL,
    @Icon VARCHAR(150) = NULL,
    @Banner VARCHAR(150) = NULL,
    @IsPublished BIT = 0,
    @IsIncludeMenu BIT = 0
AS
BEGIN
    IF @action = 'GetSubCategories'
    BEGIN
        SELECT 
            s.SubCategory_ID,
            s.SubCategory_Name,
            s.Master_SubCategory_ID,
            m.Master_SubCategory_Name,
            s.URLPath,
            s.Icon,
            s.Banner,
            s.IsPublished,
            s.IsIncludeMenu,
            s.Created_Date,
            s.Updated_Date
        FROM tbl_SubCategory s
        INNER JOIN tbl_Master_SubCategory m ON s.Master_SubCategory_ID = m.Master_SubCategory_ID
        WHERE s.IsDelete = 0 AND m.IsDelete = 0;
    END

    ELSE IF @action = 'AddSubCategory'
    BEGIN
        INSERT INTO tbl_SubCategory 
        (
            SubCategory_Name, 
            Master_SubCategory_ID, 
            URLPath, 
            Icon, 
            Banner, 
            IsPublished, 
            IsIncludeMenu,
            Created_Date,
            IsDelete
        )
        VALUES 
        (
            @SubCategory_Name, 
            @Master_SubCategory_ID, 
            @URLPath, 
            @Icon, 
            @Banner, 
            @IsPublished, 
            @IsIncludeMenu,
            GETDATE(),
            0
        );
    END

    ELSE IF @action = 'UpdateSubCategory'
    BEGIN
        UPDATE tbl_SubCategory
        SET 
            SubCategory_Name = @SubCategory_Name,
            Master_SubCategory_ID = @Master_SubCategory_ID,
            URLPath = @URLPath,
            Icon = COALESCE(@Icon, Icon),
            Banner = COALESCE(@Banner, Banner),
            IsPublished = @IsPublished,
            IsIncludeMenu = @IsIncludeMenu,
            Updated_Date = GETDATE()
        WHERE SubCategory_ID = @SubCategory_ID;
    END

    ELSE IF @action = 'DeleteSubCategory'
    BEGIN
        UPDATE tbl_SubCategory
        SET IsDelete = 1, Updated_Date = GETDATE()
        WHERE SubCategory_ID = @SubCategory_ID;
    END
END

GO
