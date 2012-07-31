/*==============================================================*/
/* User: bplt                                                    */
/*==============================================================*/
create schema bplt authorization dbo
GO

/*==============================================================*/
/* Table: Partner                                               */
/*==============================================================*/
create table bplt.Partner (
   PartnerId            int                  identity,
   Name                 nvarchar(255)        not null,
   Disabled             bit                  not null constraint DF_DISABLED_PARTNER default 0,
   Created              datetime             not null,
   Modified             datetime             null,
   constraint PK_PARTNER primary key (PartnerId)
)
GO

/*==============================================================*/
/* Index: UX_PARTNER                                            */
/*==============================================================*/
create unique index UX_PARTNER on bplt.Partner (
   Name ASC
)
GO

/*==============================================================*/
/* Table: PartnerUser                                           */
/*==============================================================*/
create table bplt.PartnerUser (
   PartnerUserId        int                  identity,
   PartnerId            int                  not null,
   UserId               int                  not null,
   constraint PK_PARTNERUSER primary key (PartnerUserId)
)
GO

/*==============================================================*/
/* Index: UX_USER                                               */
/*==============================================================*/
create unique index UX_USER on bplt.PartnerUser (
   UserId ASC
)
GO

/*==============================================================*/
/* Table: Permission                                            */
/*==============================================================*/
create table bplt.Permission (
   PermissionId         int                  identity,
   RoleId               int                  not null,
   Name                 nvarchar(255)        not null,
   Created              datetime             not null,
   Modified             datetime             null,
   constraint PK_PERMISSION primary key (PermissionId)
)
GO

/*==============================================================*/
/* Index: UX_PERMISSION                                         */
/*==============================================================*/
create unique index UX_PERMISSION on bplt.Permission (
   Name ASC,
   RoleId ASC
)
GO

/*==============================================================*/
/* Table: Role                                                  */
/*==============================================================*/
create table bplt.Role (
   RoleId               int                  identity,
   Name                 nvarchar(255)        not null,
   Created              datetime             not null,
   Modified             datetime             null,
   constraint PK_ROLE primary key (RoleId)
)
GO

/*==============================================================*/
/* Table: "User"                                                */
/*==============================================================*/
create table bplt."User" (
   UserId               int                  identity,
   RoleId               int                  not null,
   Name                 nvarchar(255)        not null,
   Email                nvarchar(255)        null,
   Deleted              bit                  not null constraint DF_DELETED_USER default 0,
   Created              datetime             not null,
   Modified             datetime             null,
   constraint PK_USER primary key (UserId)
)
GO

/*==============================================================*/
/* Table: UserPasswordCredential                                */
/*==============================================================*/
create table bplt.UserPasswordCredential (
   UserPasswordCredentialId int                  identity,
   UserId               int                  not null,
   Login                nvarchar(255)        not null,
   PasswordHash         nvarchar(100)        not null,
   PasswordSalt         nvarchar(100)        not null,
   Created              datetime             not null,
   Modified             datetime             null,
   constraint PK_USERPASSWORDCREDENTIAL primary key (UserPasswordCredentialId)
)
GO

/*==============================================================*/
/* Index: UX_LOGIN                                              */
/*==============================================================*/
create unique index UX_LOGIN on bplt.UserPasswordCredential (
   Login ASC
)
GO

/*==============================================================*/
/* Index: UX_USER                                               */
/*==============================================================*/
create unique index UX_USER on bplt.UserPasswordCredential (
   UserId ASC
)
GO

alter table bplt.PartnerUser
   add constraint FK_PARTNERUSER_USER foreign key (UserId)
      references bplt."User" (UserId)
GO

alter table bplt.PartnerUser
   add constraint FK_PARTNERUSER_PARTNER foreign key (PartnerId)
      references bplt.Partner (PartnerId)
GO

alter table bplt.Permission
   add constraint FK_PERMISSION_ROLE foreign key (RoleId)
      references bplt.Role (RoleId)
GO

alter table bplt."User"
   add constraint FK_USER_ROLE foreign key (RoleId)
      references bplt.Role (RoleId)
GO

alter table bplt.UserPasswordCredential
   add constraint FK_USERPASSWORDCREDENTI_USER foreign key (UserId)
      references bplt."User" (UserId)
GO

