interface ManageRealEstateDetail {
    reasId: number;
    reasName: string;
    reasAddress: string;
    reasPrice: number;
    reasArea: number;
    reasDescription: string;
    reasStatus: Number;
    dateStart: Date;
    dateEnd: Date;
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
        reasId: number;
        reas_Cert_Of_Land_Img_Front: string;
        reas_Cert_Of_Land_Img_After: string;
        reas_Cert_Of_Home_Ownership: string;
        reas_Registration_Book: string;
        documents_Proving_Marital_Relationship: string;
        sales_Authorization_Contract: string;
    }
  }