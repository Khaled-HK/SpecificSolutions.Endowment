// mapping أسماء الحقول من الباك اند إلى الفرونت إند
export const propertyNameMapping: Record<string, string> = {
  // Mosque fields
  'Name': 'name',
  'FileNumber': 'fileNumber', 
  'RegionId': 'regionId',
  'OfficeId': 'officeId',
  'TotalLandArea': 'totalLandArea',
  'TotalCoveredArea': 'totalCoveredArea',
  'NumberOfFloors': 'numberOfFloors',
  'OpeningDate': 'openingDate',
  'ConstructionDate': 'constructionDate',
  'MosqueDefinition': 'mosqueDefinition',
  'MosqueClassification': 'mosqueClassification',
  'SourceFunds': 'sourceFunds',
  'Definition': 'definition',
  'Classification': 'classification',
  'Unit': 'unit',
  'NearestLandmark': 'nearestLandmark',
  'MapLocation': 'mapLocation',
  'Sanitation': 'sanitation',
  'ElectricityMeter': 'electricityMeter',
  'AlternativeEnergySource': 'alternativeEnergySource',
  'WaterSource': 'waterSource',
  'BriefDescription': 'briefDescription',
  'LandDonorName': 'landDonorName',
  'PrayerCapacity': 'prayerCapacity',
  'ServicesSpecialNeeds': 'servicesSpecialNeeds',
  'SpecialEntranceWomen': 'specialEntranceWomen',
  'PicturePath': 'picturePath',
  
  // Region fields
  'CityId': 'cityId',
  'Country': 'country',
  
  // Authentication fields
  'Email': 'email',
  'Password': 'password'
} 