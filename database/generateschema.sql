﻿CREATE TABLE [dbo].[AppRoles] (
    [Id] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](max),
    CONSTRAINT [PK_dbo.AppRoles] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[AppUsers] (
    [Id] [int] NOT NULL IDENTITY,
    [AppRoleId] [int],
    [Name] [nvarchar](max),
    [LastName] [nvarchar](max),
    [Email] [nvarchar](max),
    [Password] [nvarchar](max),
    [UserName] [nvarchar](max),
    [Token] [nvarchar](max),
    CONSTRAINT [PK_dbo.AppUsers] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[Customers] (
    [Id] [int] NOT NULL IDENTITY,
    [OnlineStatus] [int] NOT NULL,
    [Name] [nvarchar](max),
    CONSTRAINT [PK_dbo.Customers] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[Chats] (
    [Id] [int] NOT NULL IDENTITY,
    [Message] [nvarchar](max),
    [SendDataTime] [datetime],
    [DeliverDateTime] [datetime],
    [CustomerId] [int],
    [MyAccountId] [int],
    [SenderType] [int] NOT NULL,
    [gapFileUniqId] [bigint],
    [SenderMySocketId] [int],
    [FileType] [int] NOT NULL,
    [MultimediaContent] [nvarchar](max),
    CONSTRAINT [PK_dbo.Chats] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[MyAccounts] (
    [Id] [int] NOT NULL IDENTITY,
    [IdentityUsername] [nvarchar](max),
    [Username] [nvarchar](max),
    [Password] [nvarchar](max),
    [OnlineStatus] [int] NOT NULL,
    [Name] [nvarchar](max),
    [Token] [nvarchar](max),
    [ParentId] [int],
    [TotalUnRead] [int] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    CONSTRAINT [PK_dbo.MyAccounts] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[MySockets] (
    [Id] [int] NOT NULL IDENTITY,
    [_myConnectionInfo] [nvarchar](max),
    [IsCustomerOrAdmin] [int],
    [Token] [nvarchar](max),
    [AdminWebsiteId] [int],
    [CustomerWebsiteId] [int],
    [CustomerId] [int],
    [MyAccountId] [int],
    CONSTRAINT [PK_dbo.MySockets] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[MyWebsites] (
    [Id] [int] NOT NULL IDENTITY,
    [BaseUrl] [nvarchar](max),
    [MyAccountId] [int],
    [WebsiteToken] [nvarchar](max),
    [IsDeleted] [bit] NOT NULL,
    CONSTRAINT [PK_dbo.MyWebsites] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[CustomerTrackInfoes] (
    [Id] [int] NOT NULL IDENTITY,
    [CustomerId] [int] NOT NULL,
    [Url] [nvarchar](max),
    [PageTitle] [nvarchar](max),
    [Descrition] [nvarchar](max),
    [CityName] [nvarchar](max),
    [Region] [nvarchar](max),
    [Time] [nvarchar](max),
    CONSTRAINT [PK_dbo.CustomerTrackInfoes] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_AppRoleId] ON [dbo].[AppUsers]([AppRoleId])
CREATE INDEX [IX_CustomerId] ON [dbo].[Chats]([CustomerId])
CREATE INDEX [IX_MyAccountId] ON [dbo].[Chats]([MyAccountId])
CREATE INDEX [IX_SenderMySocketId] ON [dbo].[Chats]([SenderMySocketId])
CREATE INDEX [IX_ParentId] ON [dbo].[MyAccounts]([ParentId])
CREATE INDEX [IX_AdminWebsiteId] ON [dbo].[MySockets]([AdminWebsiteId])
CREATE INDEX [IX_CustomerWebsiteId] ON [dbo].[MySockets]([CustomerWebsiteId])
CREATE INDEX [IX_CustomerId] ON [dbo].[MySockets]([CustomerId])
CREATE INDEX [IX_MyAccountId] ON [dbo].[MySockets]([MyAccountId])
CREATE INDEX [IX_MyAccountId] ON [dbo].[MyWebsites]([MyAccountId])
CREATE INDEX [IX_CustomerId] ON [dbo].[CustomerTrackInfoes]([CustomerId])
ALTER TABLE [dbo].[AppUsers] ADD CONSTRAINT [FK_dbo.AppUsers_dbo.AppRoles_AppRoleId] FOREIGN KEY ([AppRoleId]) REFERENCES [dbo].[AppRoles] ([Id])
ALTER TABLE [dbo].[Chats] ADD CONSTRAINT [FK_dbo.Chats_dbo.MyAccounts_MyAccountId] FOREIGN KEY ([MyAccountId]) REFERENCES [dbo].[MyAccounts] ([Id])
ALTER TABLE [dbo].[Chats] ADD CONSTRAINT [FK_dbo.Chats_dbo.MySockets_SenderMySocketId] FOREIGN KEY ([SenderMySocketId]) REFERENCES [dbo].[MySockets] ([Id])
ALTER TABLE [dbo].[Chats] ADD CONSTRAINT [FK_dbo.Chats_dbo.Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers] ([Id])
ALTER TABLE [dbo].[MyAccounts] ADD CONSTRAINT [FK_dbo.MyAccounts_dbo.MyAccounts_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[MyAccounts] ([Id])
ALTER TABLE [dbo].[MySockets] ADD CONSTRAINT [FK_dbo.MySockets_dbo.MyWebsites_AdminWebsiteId] FOREIGN KEY ([AdminWebsiteId]) REFERENCES [dbo].[MyWebsites] ([Id])
ALTER TABLE [dbo].[MySockets] ADD CONSTRAINT [FK_dbo.MySockets_dbo.MyWebsites_CustomerWebsiteId] FOREIGN KEY ([CustomerWebsiteId]) REFERENCES [dbo].[MyWebsites] ([Id])
ALTER TABLE [dbo].[MySockets] ADD CONSTRAINT [FK_dbo.MySockets_dbo.MyAccounts_MyAccountId] FOREIGN KEY ([MyAccountId]) REFERENCES [dbo].[MyAccounts] ([Id])
ALTER TABLE [dbo].[MySockets] ADD CONSTRAINT [FK_dbo.MySockets_dbo.Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers] ([Id])
ALTER TABLE [dbo].[MyWebsites] ADD CONSTRAINT [FK_dbo.MyWebsites_dbo.MyAccounts_MyAccountId] FOREIGN KEY ([MyAccountId]) REFERENCES [dbo].[MyAccounts] ([Id])
ALTER TABLE [dbo].[CustomerTrackInfoes] ADD CONSTRAINT [FK_dbo.CustomerTrackInfoes_dbo.Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers] ([Id])
CREATE TABLE [dbo].[__MigrationHistory] (
    [MigrationId] [nvarchar](150) NOT NULL,
    [ContextKey] [nvarchar](300) NOT NULL,
    [Model] [varbinary](max) NOT NULL,
    [ProductVersion] [nvarchar](32) NOT NULL,
    CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY ([MigrationId], [ContextKey])
)
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'202007050932245_AutomaticMigration', N'SignalRMVCChat.Migrations.Configuration',  0x1F8B0800000000000400ED1DDB6EE3BAF1BD40FF41F0535BEC89936C5BB481730EB2CEE620E87AB388B369DF0246621C6275F191E49C1845BFAC0FFDA4FE42A91BC5BB48DD6C6F830516B1480E8733C3E10C871CFEF7DFFF99FDF41AF8CE0B8C131485E79393A3E389034337F250B83A9F6CD2A71FFE32F9E9C7DFFE66F6D10B5E9DFBAADEFBAC1E6E1926E793E7345D9F4DA789FB0C03901C05C88DA3247A4A8FDC2898022F9A9E1E1FFF757A72328518C404C3729CD9ED264C5100F31FF8E73C0A5DB84E37C05F441EF493F23B2E59E6509DCF2080C91AB8F07CB244AB10F8B78BFBF9FC19A4471731C49D26D0DDC428DD1E15ED27CE858F00C66D09FDA78903C2304A418A313FFB9AC0651A47E16AB9C61F807FB75D435CEF09F8092C47745657371DDCF16936B869DDB002E56E92340A2C019EBC2FA935E59BB7A2F9845013D3F323A67BBACD469DD3F47C72B15EDF463E1E3BDFD7D9DC8FB37A66143F2AE1BC73D8DAEF88CC60D1CAFEBD73E61B3FDDC4F03C849B3406FE3BE7CBE6D147EEDFE0F62EFA06C3F370E3FB34CE186B5CC67CC09FBEC4D11AC6E9F6163E9523B9F626CE946D37E51B9266549B6290D761FAFE74E27CC69D83471F1291A008B24CA318FE0C431883147A5F409AC238CC60C09CA842EF5C5FD9FF556F5806F1049B380BF0FA0986ABF4F97C82FF9C3857E8157AD5971283AF21C2F311374AE30D143AF90C5ED02AC78FEB0E33040B7B8CE7C22DF4F30AC9335A1753A2E2D6435DE92A8E82EC13910852F6B08C36B19BA11E292ADC8178055316B5D9B416B52601CCA0F42180199C370154F655F24DE8721762CB75F20924E9281D7D0C00F207EFE50B48925FA3D81BBCA34CE247A15B3E33C6D55EC5B2D4557955BA49A9BC2AEDD64A79CD739D65A5BD96307E412E3CAA9ABE292C655F37A18F426CAF81749310D9CB24802D1030D8AB7538E3A97C11AE24E0A1AC524B315B222CC05CB16CF9D561B4D82E23F71B6CC28AAA26C18C94AAB1ABABD862781703F7DB75F81435A048D793E05817AB91A4EA7432633256B4D102F98C7FD3008ABE163049C06AF8E56D0943EF12A4E00ED5BA01FF86C56F7DDB4BE823EC40D7D5ED9A57926869962DB617AE1B6137DAB25D365298CB24A35133B9A38B2C75EA0AACAF900F31CD7F61F0F9F31FCDF0A95485E560B23EA54359E0F98216D043A0D570F2E641D67C1E852916D531D70B625274593278B347B1A2982F19A5B449B122A5225A5C9180175F6E8B182B3E0AEC8A42197274890437A6B893A14811D0788D281D5BD2F46D9DD0F455D4CB4CFA700C7F68B48E4673250FC9D81EC73915381143EBE5F60E0BB8FF35BC85A0713634487882ED0C88E74605E643849514089B01B5F04C2C143A6F58AB14BE3952C8F7E28C9D5ABC0A66C8112BCA344B4D59C17E11D4F94D3578A9E32429D6D0AEBDEBB4D8FE1D3E262885CD58D615E56856E55A3C49255B442B0E7665B31A394E0E5AAEDB956561EDDA554DDF966D655F0FC1161BD82174336099133EB812BF4E2A4BF826BEF00214324B5DC5B26C752F1C877D5878723CCB7966B9FC5483EDD67A28D754BDFB4B8D58A121CAD287BC2AA7C2D832C942C055B05D08740BA7A1AF216A2DA92BD2AFD368BAA3A7741EC5A5CB164133A656B5157C25C51AD6D67586F2758D9779B521A2A7A7C52245C8DA62952ADBBE2D53CABE3E80047E8D870F1BB6DDD32B3938CE6A34802352E9F0AE8A5E54AA8A95C0566B35E166ACAFD4188A3AAD7F7D656CF0EB3496E015740A9C92F04B87082A81F1A6C2947D294D39CB6D8831D4E017B0827728F587DF38BA84891BA3E264DDC05DCD319346D90CBB85AB31C64387BAF6262A621E0F569AB89298B19982DB04947A63F74CAF932B1FACEA63A6B611001A5A5F3A0E33C283B1BFC58CA3F501CB8E050C1E61CC8C69E2DC037F837F1E0BCC636B3F3D31D54FF4D53F6C922DA97B2AD2BCA0AE86E27C00B335CDE9C87C0D70776427AB5C449679339296D5EFA27AEEC8E9AB6CC877F85EDF6E095EE045720592F4224C7E85854E289B8A71E0A6A65751FC731C6DD604C49F5A4A051F0B6E2D19D8F229DCB5A33B98A48924D0BC3B29B90EF263128692B1F150642A0DF7C88391A908DC47C885A64CCFA2F79DB82B6ED7759EF53CC8DD71B4DC9D34E368D3149793F322492217E584618F5DD6472BD93E3F869ED37448BCDE4D256738F349B2C634C10B275E388E8EC4D1682093A59A815C9C196721FF41008B0D7498050110F0E7D864C0FC41612A5AF32874D11AF80D63E3DA19FA0119F1490F7CC9255C674B4C98360CDEA46BEA78B78801E988F3519A08349B5242A2971D3E68A862B0328248EFC393A5C74A7694A74DD8D34103C98D626023888D62DC263D337B4E3B921B6E8B47CD5DD57E0F2D39642BD45272544103313A3498F8C887378AFCC8476FA476B8E8D48E65A8DE8A6B66B5645FAE4F4992842976204CC220479527810626BD4BA2963B922A269AA7E6B83CB4D7CCEC4651929F4F1C6935938D6A14E1918DDAA463F12CF18E0DA13A44D86CB1488E05F5691049C291232A23E52047358E041A1C8A812409E198705C3CC0D5AF4489F12283E5730099E2C739B250F1643834A92A0FD935739C3F7167204D22B73580A5626423A61D25894563542962496BD2757D1C7927E2C3DD4451F1587593913261C8D695951E52DD7719C536928F6A0481918FDAC6A6DEBDC0341B45BA4BA6FD09CE2E2D22F508C714A256F6D0FE081215ED6D64B6EC2A70B32819C9912CAA2C82A60EBA0C2B51E248C7142991187B285345083FBBDA895B9018CAE5E302AD308C28BC7CCCAF7DBECA4EFD7F4D6019434ACA330CBC8064B097306577E59389531F1CE0E323828C0920CA3C1732104520A4010475404D80510B7F1390E24CB308205F681B1A1353470681B2F21AC19043B7122895CE6E04529F849340218E8B2151A9A9AD242E5587834AC9A82035548A13AA9A2A0D0A3F710CA2756448B4A00A33D02038C7022A71E60FC4B143352083700F4DA48236EAA47037585B94429D16520D155461260A52896F0F14E0CFBFCA48A00BA09885501822D4B3434B0445C4840145666B8F94A094998E188A4880712CA00B49C4ADFFC1A8C25E38911144BD896DB28D6D82B8C9AEF5209343721743A72214AE87F18E6C175521BA1B838984ECC4B79E2CF26D45F38DC56E8411F6118DA65E27D254174B756491ED8B99ED8C752107B7176604AA0529F84C222221743B3C267B3CF484AF35B686068A2D9D011487EC469C8600266A43B367D19E10A3E90CE9096A0D4554AEB7B1F3DD9E2612775B024C6D6FAB89539DED268E21299B4D8B3CC4E587D95491B078B600EB350A575402E3F28BB32CB217CF7F58DA27F10D0A185397A136EFC6929ED228062BC895E2AE31A657284ED22CD1D523C88E47CEBD40A8267383157E50D523E7E98A4CAC9CA2AA41F67719B2B5C82D2CD94A28015EE10107D96E447E4B46746AC4964E965A1AF82096DCC99947FE2608D5FB22EAD6C5050FBA7DF14584309B72A80BBB1E02D53831E6B960CA2395AFD89D47F94E442B1EC95B0EC323EA98240D44737AB20F7EAB20D4896F6928F557734865665B1A4CF9C91C469D6C8806537F35875427A6A521D55FCD2195B75A6930E5A7BD99553A4FB3E3B4229B73F6F34ADD749889C5DE79A2E1B025DF913A5559C65D999E6DA8B660B8B4D930CC2689396910E4A3391C36F1260D8C2D318728A4E3A4810A85E670E918050D5217BBD0D08F3EFFC1D0507730444FC7EA120C4F45792E1B1D342E9D270D902BB2C5903E9C27E2A93BBAA7865C6701A521D65F2DB822E6FD64782316EF8D2AA236077AD7477578C65E2969DA0EA399C454902C2CBED4CEA61121B681D49F9DB5CB45B70F8B4D4DA1EA74134B21D599271D365406461627AAC042BEEAFC278C60D59FF74827A8F7AD3AAB8432D6DA4623A89A0EA3102449E6686092621B6110D2C9B14221148F3B8DF8CB398CC7DB7071470D55723F4366117582BD5B336B6713561388E93C63AB730D6DA6ACB2ED30739664DCA241908FBB33B3D94C5B3440B6E4BB5C4EC4BDF5C1B63B945D58EC7B68600C23B4FDAA2E41FC2D459FCAA2C49A50E4B38D4F5DE74962DDE9FABB059D482A24864AE4AB39A42AD7110DA7FA66B1D00ADB04AABD8141279E107EE2AB90DE49188A0B37CDCAD04FF3239A422CA8A8327130695E9097C58196DB2485C15156E168F98B3FF751EEF056151620444F30498B1C1793D3E39353EED5CDFD7901739A249E2F099D51A944A481A23152BFA18CA88DC9DD3A64DD0F5F40EC3E83F8770178FD3D0D494CC065FD66E3F7412DE15D44247DA6E33AF4E0EBF9E49F79AB33E7FA1F0FA4E13BE726C673E2CC3976FEA527716FBC1201F16F287602C6BC93D80912FF80452760FC7B879D8031F952879826F208CCC1CE13D90B2179477BA69DC438C8C1929C7BFFAC93B8CBDE38F3309A69FEDB0E96E2CDB3B6E0C4ECA4A62AB86ED945074B122F9B224035ED8281F82C5B9B99257D86ED11AD3858E60889EFB299D2856FDF8538FC336F6D48A37CD26D080DA4087A1CAC1A523DAFD5792DEF0D58AF56C68E17BAFE2C15199DD847AC4CA773D5AECB3496BC85D586AA4202FA47D404C4FEB99FEF63DE2ADFD7E92443CA37745A2C33FD49B6FC951C63578E69DD45CA954FEED89A34BD62F3FF6259B57833E5FB98EADC1B259DA6D2EECD61D98B261D75D6708B4643C0E160456A48DD61FBD84447A3877B50A21334F1D1884EE0F887213A01631F7FE86673A25E7769BAE79AEE27B774912E41964261C85CD223E58E56DCC010BB3AB85CD1BDE4AE2B12558C953E537D66B2FFF42BF203E3623FFB9076CE36DB734F5979ABDC2063A6BD549FB8E95D043407F224D3FF607336BF0943EFC270A0D996FB48B03CEE8260C193B1D68343CE98DC533ADB5DA882114D032BB1DB07F3A04DAEE39E44C16A8D394C61B05984F6491A4C7314F79BE3BA38C86602F130A5811EA64987879573B8976CB1235A089ABBDEBBB310F627DDAB79DEE03E18BF0BC3604C0168E522EC5E082C72FEF696E197CA25A4CFEC79F0426172FD62E7D2C19D92AF9010B21BF16C95E6E5D5A5E52DCEC49F4FBCC78CF3C586B72A4BAA2A69AF2E67AF02BE3C792A0FBF1668A183BA48D68332DD9634DFAF32DDAF14B4343D1B0F96B2A104D85499AC034D063CB1974A554B3AA98AE47DA8D2AA895D10D740D207299377A2CC6AA862B46596611DEB493549F723A62396A51F96681049F8866D27CD80BD2FC986E5191CB5C3A4E75D9DD8A58F010E934B589EAF533B447E76B217C3FB1CEA10C982F772C0BDE70136427524C11D32CF6F8B193A3C37874DE1DB6AC856C2BF47C9795B0D96D9003269BF1F2978A5B950773267074CB16B3FC8A1E7EBA0E9735BF05465328E992257BC8B8C7D3E3C7150509C0BC2CE6782563588597D52B9065AD5C9D0AC1C4F0EA3AA8A70732A051E76052FE2143D0137C5C52E4C1214AE26CE3DF03730BBE3F808BDEBF06693AE37291E320C1EFD2D4D8CCC79D5F59FE70166719EDDACB35F491F43C068A2EC46D54DF861837C8FE07D2539ABA8009179C5E589C08C9769763270B525903E47A121A0927CC499BF83C1DAC7C0929B70095E601BDCB0D9FE09AE80BBADAE94AB8134338225FBEC1281550C82A48451B7C73FB10C7BC1EB8FFF03C568F38DFDB20000 , N'6.1.0-30225')

