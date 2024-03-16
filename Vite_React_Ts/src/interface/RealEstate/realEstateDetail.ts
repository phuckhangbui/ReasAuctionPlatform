interface realEstateDetail {
  reasId: number;
  reasName: string;
  reasAddress: string;
  reasPrice: string;
  reasArea: number;
  reasDescription: string;
  reasStatus: number;
  reasTypeName: string;
  dateStart: Date;
  dateEnd: any;
  accountOwnerId: number;
  accountOwnerName: string;
  type_REAS_Name: string;
  dateCreated: Date;
  photos: [
    {
      reasPhotoId: number;
      reasPhotoUrl: string;
      reasId: number;
    }
  ];
  detail: {
    reasId: string;
    reas_Cert_Of_Land_Img_Front: string;
    reas_Cert_Of_Land_Img_After: string;
    reas_Cert_Of_Home_Ownership: string;
    reas_Registration_Book: string;
    documents_Proving_Marital_Relationship: string;
    sales_Authorization_Contract: string;
  };
}
