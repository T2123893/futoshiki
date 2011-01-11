/*
 * $Id$
 * 
 * Coursework – Futoshiki.DAL
 *
 * This file is the result of my own work. Any contributions to the work by 
 * third parties, other than tutors, are stated clearly below this declaration. 
 * Should this statement prove to be untrue I recognise the right and duty of 
 * the Board of Examiners to take appropriate action in line with the university's 
 * regulations on assessment. 
 */


namespace DAL
{
    public static class Const
    {
        /// <summary>
        /// Default connection string key. It should be configured in web.xml
        /// </summary>
        public const string ConnStr = "FutoshikiConnectionString";

        /// <summary>
        /// dbo.buzzel_Grids.Id
        /// </summary>
        public const string Id = "@Id";
        
        /// <summary>
        /// dbo.buzzel_Cells.GridId
        /// </summary>
        public const string Gid = "@GridId";

        /// <summary>
        /// dbo.buzzel_Grids.UserId
        /// </summary>
        public const string Uid = "@UserId";

        /// <summary>
        /// dbo.buzzel_Grids.Scale
        /// </summary>
        public const string Scale = "@Scale";

        /// <summary>
        /// dbo.buzzel_Grids.Status
        /// </summary>
        public const string Status = "@Status";

        /// <summary>
        /// dbo.buzzel_Cells.Row
        /// </summary>
        public const string Row = "@Row";

        /// <summary>
        /// dbo.buzzel_Cells.Col
        /// </summary>
        public const string Col = "@Col";

        /// <summary>
        /// dbo.buzzel_Cells.Val
        /// </summary>
        public const string Val = "@Val";

        /// <summary>
        /// dbo.buzzel_Cells.IsWritable
        /// </summary>
        public const string IsWritable = "@IsWritable";
        
        /// <summary>
        /// create a cell.
        /// </summary>
        public const string CrtCell = 
            "INSERT INTO puzzel_Cells (Id,GridId,Row,Col,Val,IsWritable) " + 
            "VALUES (NEWID(),@GridId,@Row,@Col,@Val,@IsWritable)";
        
        /// <summary>
        /// create a grid.
        /// </summary>
        public const string CrtGrid = 
            "INSERT INTO puzzel_Grids (Id,UserId,Scale,Status) " + 
            "VALUES (@Id,@UserId,@Scale,@Status)";

        /// <summary>
        /// Select new game for current user.
        /// </summary>
        public const string Read0 =
            "SELECT a.UserId,a.Id,a.Scale,a.Status,b.Row,b.Col,b.Val,b.IsWritable " +
            "FROM puzzel_Grids a, puzzel_Cells b " +
            "WHERE a.Id=b.GridId AND a.Status=0 " +
            "AND a.UserId=@UserId AND a.Scale=@Scale";    

        /// <summary>
        /// Select ongoing game for current user.
        /// </summary>
        public const string Read1 =
            "SELECT a.UserId,a.Id,a.Scale,a.Status,b.Row,b.Col,b.Val,b.IsWritable " +
            "FROM puzzel_Grids a, puzzel_Cells b " +
            "WHERE a.Id=b.GridId AND a.Status=1 " +
            "AND a.UserId=@UserId AND a.Scale=@Scale ";

        /// <summary>
        /// Update a futoshiki's status.
        /// </summary>
        public const string UpdtGrids = 
            "UPDATE puzzel_Grids SET Status=@Status " +
            "WHERE Id=@Id AND UserId=@UserId";

        /// <summary>
        /// Update a cell.
        /// </summary>
        public const string UpdtCells =
            "UPDATE puzzel_Cells SET Val=@Val FROM puzzel_Grids " +
            "WHERE puzzel_Grids.Id=puzzel_Cells.GridId " +
            "AND puzzel_Grids.UserId=@UserId AND puzzel_Grids.Scale=@Scale" +
            "AND puzzel_Cells.GridId=@GridId" +
            "AND puzzel_Cells.Row=@Row AND puzzel_Cells.Col=@Col";
    }
}







