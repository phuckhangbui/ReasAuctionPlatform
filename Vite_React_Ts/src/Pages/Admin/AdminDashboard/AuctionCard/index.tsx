import {
  Card,
  CardHeader,
  CardBody,
  CardFooter,
  Typography,
} from "@material-tailwind/react";
import React from "react";

interface NewsCardProps {
  newsItem: news | undefined;
}

const MAX_SUMMARY_LENGTH = 100; // Độ dài tối đa cho summary trước khi hiển thị dấu "..."

const NewsCard: React.FC<NewsCardProps> = ({ newsItem }) => {
  const formatDate = (dateString: Date): string => {
    const dateObject = new Date(dateString);
    return `${dateObject.getFullYear()}-${(
      "0" +
      (dateObject.getMonth() + 1)
    ).slice(-2)}-${("0" + dateObject.getDate()).slice(-2)} ${(
      "0" + dateObject.getHours()
    ).slice(-2)}:${("0" + dateObject.getMinutes()).slice(-2)}:${(
      "0" + dateObject.getSeconds()
    ).slice(-2)}`;
  };

  return (
    <Card className="w-full max-h-130 ">
      <CardHeader
        floated={false}
        shadow={false}
        color="transparent"
        className="m-0 rounded-none"
      >
        <img src={newsItem?.thumbnail} alt={newsItem?.newsTitle} />
      </CardHeader>
      <CardBody>
        <Typography variant="h4" color="blue-gray" style={{fontSize:18}}>
          {newsItem?.newsTitle}
        </Typography>
        <Typography variant="lead" color="gray" className="mt-3 font-normal" style={{fontSize:14}}>
          {newsItem?.newsSumary && newsItem?.newsSumary.length > MAX_SUMMARY_LENGTH
            ? `${newsItem?.newsSumary.slice(0, MAX_SUMMARY_LENGTH)}...`
            : newsItem?.newsSumary}
        </Typography>
      </CardBody>
      <CardFooter className="flex items-center justify-between">
        <div className="flex items-center -space-x-3"></div>
        <Typography className="font-normal" style={{fontSize:14}}>
          {newsItem && formatDate(newsItem.dateCreated)}
        </Typography>
      </CardFooter>
    </Card>
  );
};

export default NewsCard;
