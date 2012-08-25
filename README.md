# Overview

amss-boilerplate (Asp.net Mvc Service Stack Boilerplate) is our attempt at a resusable code base for building common enterprise applications (web applications, rest/web services, smart-clients etc). It uses a variety of frameworks including ServiceStack.net, Fluent Validation, Log4Net, and others to support basic development best practices. We even included build and CI scripts to help keep your code solid. Our goal is to develop a clean application shell created in minutes rather than the days it typically takes to start up a new project.

**ServiceStack.Net**

We chose [ServiceStack.net](http://www.servicestack.net/) over WPF because we felt this open source framework better greatly simplified building restful services in .net. Because the vast majority of what we do today is restful, we don't need most of the WCF features.

## Types of Applications Supported

Restful Services
Asp.net MVC web app
Worker Processes

## Requirements
VS 2010
.NET Framework 4
Code Contracts 
ASP.NET MVC 4 RC
msbuild (for automated build system, path to msbuild.exe should be in system paths)

## Using the Rename Command

The boilerplate code supports renaming of namespaces for your particular project needs.

- *boilerplate.cmd* – renames solution, projects, namespaces to the name specified in boilerplate.proj file parameters: ProductName, ProductDatabase, ProductDatabaseSchema, ProductCompanyName
- *boilerplate.clean.cmd*

## Solution structure

Solution folders are used for organizing solution structure and separating projects depend on its purpose/layer.  We follow .NET application architecture guide practices for naming folders.   

### Cross-cutting

- *Amss.Boilerplate.Initialization* - registers project container extensions, configures unity logging (enterprise library 5, unity application block)  
- *Amss.Boilerplate.Common* - set of common functionality used across all parts of application (helpers, utilities, identities etc ) 
- *Shell.cs* - configures/destroys unity container and initializes service locator. Per process (Dependency Injections, Service Locator)
- *UnitOfWork.cs* - enables/disables unit of work life time manager. Per thread (Unit Of Work)     
- *Amss.Boilerplate.Data* – set of domain objects (Anemic Domain Object model). 

### Persistence (Data access)

- *Amss.Boilerplate.Migrations* – database install/updates (FluentMigrator) 
- *Amss.Boilerplate.Persistence* – repository pattern implementation, holds everything related to data access – ORM mapping, domain entities, interceptors etc. (FluentNhibernate, NHibernate ).  CQRS principle is used for separating command and queries. 
Business
- *Amss.Boilerplate.Business* - business managers - holds business logic and exposes persistence operations to UI (There is no direct access from UI to persistence). 

### Presentation
The main purpose of presentation projects is to show to deal with Shell, UnitOfWork, business services and data. Also they contain major principles for project structure, naming conventions, exception handling etc.

- *Amss.Boilerplate.Api* – REST service built on servicestack framework. 
- *Amss.Boilerplate.Web* – asp.net mvc 4 web application. Contains correct exception handling (handling 404), some design how to work with client grids, data updates, validation etc  


### Tests
- *Amss.Boilerplate.Tests* – unit and integration tests

### Build and deployment 

- *Build folders* - contains everything required for continuous integration 

## Design issues

### Exception handling

Only expected exceptions are handled on specific layer. Unhandled exceptions or exceptions which cannot be processed are thrown to higher layer. Top layer (presentation) handles unhandled exceptions, logs them and inform user somehow.     
Validation

- Application has few validation levels: client-side validation (javascript), mvc model validation on presentation layer (Data Annotations, FluentValidation could be used also), business validation on business layer, database validation (constrains, reference integrity, primary/foreign keys etc)      

## Working Client Side

### Using datatables/jqGrid with oData (Data Services)

Binding oData (Data Service) with client side grid (Datatables, jqGrid) when IQueryable is exposed from persistence. 

*Pros:*
- Paging, sorting, filtering on presentation side (mvc) is done automatically 

*Cons:*
- linq to nhibernate doesn't support all types of expressions (e.g. $filter=startswith([Column], '[text]')) doesn't work
- separate views should be implemented for each grid because using of business entities may lead to problems with loading referenced objects when showing child object data in one view
- [datatables] more client side pre/post processing to convert grid request json to oData get format and convert oData response to datatables format (it might be solved by implementing datatables extension)
- [datatables] datatables grid has weird structure of internal json objects (e.g. aaData, or holding sorting columns in Column_X view) which complicates data processing and transformation 
- client has to match exact business properties by name for sorting/searching what may complicate refactoring 

You may want to use oData for client side only if you already have or need oData for other purposes and want to reuse server part instead of implementing mvc controller part. But using mvc controllers for getting grid data is a good idea most of the time because there are number of helpers/wrappers for datatables/jqGrid which allows to use strongly typed model instead of implementing ton of transformations on client side.

### Datables vs jqGrid

- datatables is compatible with bootstrap 
- datatables is resized without additional coding (jqGrid requires subscribing and handling onresize event)
- I didn’t find any styling issues using jqGrid except input with but it’s not a big deal to fix (but it wasn’t used a lot)
- datatables has worse documentation, event model seems worse also, internal data structures look weird sometimes; jqGrid looks more powerful and well designed (but it’s subjective since don’t have much experience with datatables)   