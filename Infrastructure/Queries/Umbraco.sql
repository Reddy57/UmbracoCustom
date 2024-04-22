SELECT *
FROM [umbracocms].[dbo].[umbracoNode]

-- WHERE nodeObjectType ='c66ba18e-eaf3-4cff-8a22-41b16d66a972'

WHERE text LIKE '%afs%';


-- - **Document Types**: `a87ee5f8-e6df-45b9-9fe3-28a3b86e6627`
-- - **Media Types**: `4ea4382b-2f5a-4c2b-9587-ae9b3cf3602e`
-- - **Member Types**: `9ab673f8-1154-4fc2-8b7d-00afad17da55`
-- - **Member Groups**: `366e63b9-880f-4e13-a61c-98069b029728`
-- - **Data Types**: `30a2a501-1978-4ddb-a57b-f7efed43ba3c`




-- Media Types
SELECT *
from umbracoNode
where nodeObjectType = '4ea4382b-2f5a-4c2b-9587-ae9b3cf3602e'

-- Member Types
SELECT *
from umbracoNode
where nodeObjectType = '9ab673f8-1154-4fc2-8b7d-00afad17da55'



SELECT *
from cmsMember2MemberGroup


-- Members with custom properties and Roles/Groups

-- Get the Roles Look up by GUID Member Groups
SELECT *
from umbracoNode
WHERE nodeObjectType = '366e63b9-880f-4e13-a61c-98069b029728'


SELECT *
from umbracoNode
WHERE Id = 1044

2055
2066
2069
2070

SELECT *
from cmsMember
where nodeId in(2069, 2070)

SELECT *
from umbracoNode
WHERE id in(2069, 2070)

SELECT *
from umbracoPropertyData
where propertyTypeId in (SELECT id
from cmsPropertyType
where Alias in ('firstName', 'lastName'))

Select *
from umbracoContentVersion
where nodeId in(2069, 2070)


SELECT *
from cmsPropertyType
where Alias in ('firstName', 'lastName');




SELECT *
from cmsContentNu


Select cc.[current]
from umbracoContentVersion cc

SELECT *
FROM [umbracocms].[dbo].[cmsMemberType]

--Select * from cmsPropertyTypeGroup

-- Members
SELECT *
from umbracoNode
where nodeObjectType = '39eb0f98-b348-42a1-8662-e7eb18487560'



SELECT
    n.id AS NodeId,
    n.text AS MemberName,
    m.Email,
    m.LoginName,
    pd.varcharValue AS PropertyValue,
    pt.Alias AS PropertyAlias
FROM
    umbracoNode AS n
    INNER JOIN
    cmsMember AS m ON n.id = m.nodeId
    LEFT JOIN
    umbracoPropertyData AS pd ON n.id = pd.versionId -- assuming versionId links to nodeId; adjust based on actual schema
    LEFT JOIN
    cmsPropertyType AS pt ON pd.propertyTypeId = pt.id
WHERE 
    n.id = 2069
    AND pt.Alias IN ('firstName', 'lastName');




SELECT *
from umbracoNode
WHERE id  in( 2066, 2055)




SELECT *
from cmsPropertyTypeGroup


SELECT *
from umbracoDataType;

-- Member Types
SELECT *
from umbracoNode
where nodeObjectType = '9ab673f8-1154-4fc2-8b7d-00afad17da55'




SELECT
    n.id AS NodeId,
    n.text AS MemberName,
    m.Email,
    m.LoginName,
    pt.Alias AS PropertyAlias,
    v.[current],
    CASE 
        WHEN pd.varcharValue IS NOT NULL THEN pd.varcharValue
        WHEN pd.textValue IS NOT NULL THEN pd.textValue
        ELSE 'No Value'
    END AS PropertyValue
FROM
    umbracoNode AS n
    JOIN
    cmsMember AS m ON n.id = m.nodeId
    LEFT JOIN
    umbracoContentVersion AS v ON n.id = v.nodeId
    LEFT JOIN
    umbracoPropertyData AS pd ON v.id = pd.versionId
    LEFT JOIN
    cmsPropertyType AS pt ON pd.propertyTypeId = pt.id
WHERE 
    n.id = 2069
    AND pt.Alias IN ('firstName', 'lastName') AND v.[current] = 1