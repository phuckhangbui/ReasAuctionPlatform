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
  detail: string
}
