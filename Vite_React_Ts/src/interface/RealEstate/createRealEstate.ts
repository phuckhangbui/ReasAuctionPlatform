interface createRealEstate {
  reasName: string;
  reasAddress: string;
  reasPrice: number;
  reasArea: number;
  reasDescription: string;
  dateEnd: Date;
  type_Reas: number;
  photos: [
    {
      reasPhotoUrl: string;
    }
  ];
  detail: {
    reas_Cert_Of_Land_Img_Front: string;
    reas_Cert_Of_Land_Img_After: string;
    reas_Cert_Of_Home_Ownership: string;
    reas_Registration_Book: string;
    documents_Proving_Marital_Relationship: string;
    sales_Authorization_Contract: string;
  };
}
