-- Delete all of the data

DELETE FROM park;
DELETE FROM reservation;
DELETE FROM site;
DELETE FROM campground;

-- Insert a fake 
INSERT INTO department
(name)
VALUES
('Test Department'),
('Cosmetics');

DECLARE @newDepartmentId int = (SELECT @@IDENTITY);

-- Insert a fake employee
INSERT INTO employee
(job_title, last_name, first_name, gender, hire_date, birth_date, department_id)
VALUES
('Head of Technology', 'Smith', 'Jeff', 'M', '02/02/2020', '03/04/1990', (SELECT department_id FROM department WHERE name = 'Test Department')),
('VP of Sales', 'Doe', 'Jane', 'F', '08/21/2018', '12/25/1991', (SELECT department_id FROM department WHERE name = 'Cosmetics'));

DECLARE @newEmployeeId int = (SELECT @@IDENTITY);

-- Insert a fake project
INSERT INTO project (name, from_date, to_date)
VALUES
('Mission B', '01/02/2004', '06/13/2013'),
('Capstone Project', '04/18/2009', '09/21/2012')

DECLARE @newProjectId int = (SELECT @@IDENTITY);

INSERT INTO project_employee (project_id, employee_id)
VALUES
(@newProjectId, @newEmployeeId)

---- Assign the fake city to be the capital of the fake country
--UPDATE country SET capital = @newCityId;

-- Return the id of the fake city
SELECT @newDepartmentId as newDepartmentId, @newEmployeeId as newEmployeeId, @newProjectId as newProjectId;

