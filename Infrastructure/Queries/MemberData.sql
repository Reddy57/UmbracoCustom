SELECT
    n.id AS NodeId,
    n.text AS MemberName,
    m.Email,
    m.LoginName,
    pt.Alias AS PropertyAlias,
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
    --AND v.current = 1
    LEFT JOIN
    umbracoPropertyData AS pd ON v.id = pd.versionId
    LEFT JOIN
    cmsPropertyType AS pt ON pd.propertyTypeId = pt.id
WHERE 
    n.id = 2069
    AND pt.Alias IN ('firstName', 'lastName');
